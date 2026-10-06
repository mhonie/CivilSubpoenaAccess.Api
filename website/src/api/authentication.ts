import axios from "./axios";


export type UserLoginInfo = {
  userName: string;
  password: string;
};

export type LoginResponse = {
  token: string;
};

export type LoginErrorResponse = {
  title: string;
  status: number;
  detail: string;
};

export async function login(loginInfo: UserLoginInfo): Promise<LoginResponse> {
  const response = await axios.post<LoginResponse>("/login", loginInfo);

  return response.data;
}