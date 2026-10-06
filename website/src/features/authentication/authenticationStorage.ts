const authenticationStorageKey = "civilSubpoenaAccessAuthentication";


export function storedWebToken(): string | null {
  return sessionStorage.getItem(authenticationStorageKey);
}

export function storeWebToken(webToken: string) {
  sessionStorage.setItem(authenticationStorageKey, webToken);
}

export function removeStoredWebToken() {
  sessionStorage.removeItem(authenticationStorageKey);
}