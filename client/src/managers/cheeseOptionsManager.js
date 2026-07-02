import { apiFetch } from "./apiClient";

export const getCheeseOptions = async () => {
  const response = await apiFetch("/cheeseoptions");

  if (!response.ok) {
    throw new Error("Failed to load cheese options");
  }

  return response.json();
};