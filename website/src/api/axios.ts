import axios, {
  type AxiosError,
  type InternalAxiosRequestConfig
} from "axios";

import {
  removeStoredWebToken,
  storedWebToken
} from "../features/authentication/authenticationStorage";


type ErrorResponse = {
  title?: string;
  detail?: string;
};

type RetryRequestConfig = InternalAxiosRequestConfig & {
  retryCount?: number;
};

const maximumRetries = 2;

export class ApiError extends Error
{
  public readonly status: number | null;

  constructor
  (
    message: string,

    status: number | null
  )
  {
    super(message);

    this.name = "ApiError";

    this.status = status;
  }
}

function ErrorMessage(error: unknown): string
{
  if (!axios.isAxiosError<ErrorResponse | string>(error))

    return error instanceof Error

      ? error.message

      : "An unexpected error occurred.";

  if (typeof error.response?.data == "string")

    return error.response.data;

  return error.response?.data.detail

    ?? error.response?.data.title

    ?? (error.response

      ? "The server was unable to complete the request."

      : "Unable to contact the server. Please try again.");
}

function RetryableRequest(error: AxiosError): boolean
{
  if (axios.isCancel(error) || !error.config)

    return false;

  const method = error.config.method?.toLowerCase();

  const readOnlyRequest = method == "get"

    || method == "head"

    || method == "options";

  const graphQuery = method == "post"

    && error.config.url == "/graphql";

  if (!readOnlyRequest && !graphQuery)

    return false;

  const status = error.response?.status;

  return status === undefined

    || status == 408

    || status == 429

    || status >= 500;
}

function RetryDelay(error: AxiosError, retryCount: number): number
{
  const retryAfter = error.response?.headers["retry-after"];

  if (retryAfter)
  {
    const retryAfterSeconds = Number(retryAfter);

    if (!Number.isNaN(retryAfterSeconds))

      return retryAfterSeconds * 1000;

    const retryAfterDate = Date.parse(retryAfter);

    if (!Number.isNaN(retryAfterDate))

      return Math.max(retryAfterDate - Date.now(), 0);
  }

  return 500 * 2 ** (retryCount - 1);
}

function Delay(milliseconds: number): Promise<void>
{
  return new Promise(resolve =>

    window.setTimeout(resolve, milliseconds)
  );
}

const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL
});

axiosInstance.interceptors.request.use(requestConfig =>
{
  const webToken = storedWebToken();

  if (webToken && requestConfig.url != "/login")

    requestConfig.headers.set(
      "Authorization",

      `Bearer ${webToken}`
    );

  return requestConfig;
});

axiosInstance.interceptors.response.use(
  response => response,

  async (error: unknown) =>
  {
    if (!axios.isAxiosError(error) || !RetryableRequest(error))

      return Promise.reject(error);

    const requestConfig = error.config as RetryRequestConfig;

    requestConfig.retryCount = (requestConfig.retryCount ?? 0) + 1;

    if (requestConfig.retryCount > maximumRetries)

      return Promise.reject(error);

    await Delay(RetryDelay(error, requestConfig.retryCount));

    return axiosInstance(requestConfig);
  }
);

axiosInstance.interceptors.response.use(
  response => response,

  (error: unknown) =>
  {
    const axiosError = axios.isAxiosError(error)

      ? error

      : null;

    const status = axiosError?.response?.status ?? null;

    const loginRequest = axiosError?.config?.url == "/login";

    if (status == 401 && !loginRequest)
    {
      removeStoredWebToken();

      window.location.assign(import.meta.env.BASE_URL);
    }

    if (status == 403 && !loginRequest)

      window.location.assign(
        `${import.meta.env.BASE_URL}access-denied`
      );

    return Promise.reject(
      new ApiError
      (
        ErrorMessage(error),

        status
      )
    );
  }
);

export default axiosInstance;