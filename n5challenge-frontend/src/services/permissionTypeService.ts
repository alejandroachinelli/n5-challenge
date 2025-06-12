import axios from 'axios';
import type { PermissionType } from '../types/permission';

const API_BASE = 'http://localhost:8080/api/PermissionTypes';

export const getAllPermissionTypes = async (): Promise<PermissionType[]> => {
  const response = await axios.get<{ data: PermissionType[] }>(API_BASE);
  return response.data.data;
};