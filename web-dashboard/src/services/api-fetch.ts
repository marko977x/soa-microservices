
const MINIMUM_PASSWORD_LENGTH = 6;
const MINIMUM_USERNAME_LENGTH = 6;

export function apiFetch(method: string, url: string, data: any = null): Promise<Response> {
  let params: RequestInit = {
    method: method,
    headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json, text/plain, */*'
    },
    body: data ? JSON.stringify(data) : undefined
  }

  return fetch(url, params);
}