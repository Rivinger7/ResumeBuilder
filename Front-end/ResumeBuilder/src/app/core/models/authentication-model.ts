export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
}

export interface LoginWithGoogleRequest {
  idToken: string;
}

// Response Type
export type LoginResponse = string;

export type RefreshTokenResponse = string;

export type RegisterResponse = string;
