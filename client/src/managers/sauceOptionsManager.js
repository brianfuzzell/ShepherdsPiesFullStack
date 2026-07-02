import { apiFetch } from "./apiClient";

export const getSauceOptions = async () => {
  const response = await apiFetch("/sauceoptions");

  if (!response.ok) {
    throw new Error("Failed to load sauce options");
  }

  return response.json();
};