import { apiFetch } from "./apiClient";

export const getToppings = async () => {
  const response = await apiFetch("/toppings");

  if (!response.ok) {
    throw new Error("Failed to load toppings");
  }

  return response.json();
};