import { apiFetch } from "./apiClient";

export const login = async (email, password) => {
  const response = await apiFetch("/login", {
    method: "POST",
    body: JSON.stringify({ email, password }),
  });

  if (!response.ok) {
    throw new Error("Invalid email or password");
  }

  return response.json();
};

export const logout = () => apiFetch("/logout", { method: "POST" });

export const getProfile = async () => {
  const response = await apiFetch("/login/profile");

  if (!response.ok) {
    return null;
  }

  return response.json();
};