import axios from 'axios';
import type { PermissionDto } from '../types/permission';

const API_BASE = 'http://localhost:8080/api/Permissions';

const api = axios.create({
  baseURL: API_BASE,
});

export const getAllPermissions = async (): Promise<PermissionDto[]> => {
  const response = await axios.get<{ data: PermissionDto[] }>(API_BASE);
  return response.data.data;
};

export const createPermission = async (permission: Omit<PermissionDto, 'id' | 'permissionType'>) => {
  const response = await api.post('/Permissions', permission);
  return response.data;
};

export const modifyPermission = async (id: number, permission: Omit<PermissionDto, 'id' | 'permissionType'>) => {
  const response = await api.put(`${id}`, permission);
  return response.data;
};

export const deletePermission = async (id: number) => {
  const response = await api.delete(`${id}`);
  return response.data;
};