import { apiFetch } from "./apiClient";

export const getSizes = async () => {
  const response = await apiFetch("/sizes");

  if (!response.ok) {
    throw new Error("Failed to load sizes");
  }

  return response.json();
};