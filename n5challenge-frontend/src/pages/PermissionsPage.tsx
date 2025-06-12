import {
  Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Paper, Typography,
  IconButton, CircularProgress, Snackbar, Alert, Box
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getAllPermissions, deletePermission } from '../services/permissionService';
import type { PermissionDto } from '../types/permission';
import ConfirmDialog from '../components/ConfirmDialog';

const PermissionsPage = () => {
  const [permissions, setPermissions] = useState<PermissionDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [successMsg, setSuccessMsg] = useState('');
  const [openConfirm, setOpenConfirm] = useState(false);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const navigate = useNavigate();

  const fetchPermissions = async () => {
    setLoading(true);
    try {
      const data = await getAllPermissions();
      setPermissions(data);
    } catch (error) {
      console.error('Error al cargar los permisos', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPermissions();
  }, []);

  const openDeleteDialog = (id: number) => {
    setSelectedId(id);
    setOpenConfirm(true);
  };

  const confirmDelete = async () => {
    if (!selectedId) return;

    try {
      await deletePermission(selectedId);
      setSuccessMsg('Permiso eliminado correctamente');
      await fetchPermissions();
    } catch (error) {
      alert('Error al eliminar el permiso');
    } finally {
      setOpenConfirm(false);
      setSelectedId(null);
    }
  };

  return (
    <Box sx={{ mt: 6, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
      <Typography variant="h4" gutterBottom>
        Lista de Permisos
      </Typography>

      {loading ? (
        <CircularProgress sx={{ mt: 4 }} />
      ) : (
        <TableContainer component={Paper} sx={{ maxWidth: 1000, width: '100%' }}>
          <Table>
            <TableHead>
              <TableRow sx={{ backgroundColor: '#1976d2' }}>
                <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Nombre</TableCell>
                <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Apellido</TableCell>
                <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Tipo de Permiso</TableCell>
                <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Fecha</TableCell>
                <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Acciones</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {permissions.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={5} align="center">
                    No hay permisos disponibles
                  </TableCell>
                </TableRow>
              ) : (
                permissions.map((p) => (
                  <TableRow key={p.id} hover>
                    <TableCell>{p.employeeName}</TableCell>
                    <TableCell>{p.employeeLastName}</TableCell>
                    <TableCell>{p.permissionTypeDescription}</TableCell>
                    <TableCell>{new Date(p.permissionDate).toLocaleDateString()}</TableCell>
                    <TableCell>
                      <IconButton color="primary" onClick={() => navigate(`/edit/${p.id}`)}>
                        <EditIcon />
                      </IconButton>
                      <IconButton color="error" onClick={() => openDeleteDialog(p.id)}>
                        <DeleteIcon />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      <ConfirmDialog
        open={openConfirm}
        description="¿Deseás eliminar este permiso? Esta acción no se puede deshacer."
        onClose={() => setOpenConfirm(false)}
        onConfirm={confirmDelete}
      />

      <Snackbar
        open={!!successMsg}
        autoHideDuration={3000}
        onClose={() => setSuccessMsg('')}
      >
        <Alert severity="success" onClose={() => setSuccessMsg('')} sx={{ width: '100%' }}>
          {successMsg}
        </Alert>
      </Snackbar>
    </Box>
  );
};

export default PermissionsPage;