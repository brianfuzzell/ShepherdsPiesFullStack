import { apiFetch } from "./apiClient";

export const getEmployees = async () => {
  const response = await apiFetch("/employees");

  if (!response.ok) {
    throw new Error("Failed to load employees");
  }

  return response.json();
};