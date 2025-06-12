export interface ApiResponse<T> {
  data: T;
  message: string | null;
  isSuccess: boolean;
  errors: string[];
}

export interface PermissionDto {
  id: number;
  employeeName: string;
  employeeLastName: string;
  permissionTypeId: number;
  permissionTypeDescription: string;
  permissionDate: string;
}

export interface PermissionType {
  id: number;
  description: string;
}