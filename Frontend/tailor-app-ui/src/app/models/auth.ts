export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role: string;
  shopId: number;
}

export interface AuthResponse {
  token: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  shopId: number;
  expiry: string;
}

export interface AppUser {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  shopId: number;
  isActive: boolean;
  createdDate: string;
}