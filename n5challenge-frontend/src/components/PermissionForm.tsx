import {
  Box,
  Button,
  MenuItem,
  TextField,
  Typography,
  Snackbar,
  Alert,
  CircularProgress,
  Paper,
} from '@mui/material';
import { useState, useEffect } from 'react';
import axios from 'axios';
import { getAllPermissionTypes } from '../services/permissionTypeService';
import { useNavigate } from 'react-router-dom';
import type { PermissionType } from '../types/permission';

export const PermissionForm = () => {
  const [name, setName] = useState('');
  const [surname, setSurname] = useState('');
  const [permissionTypeId, setPermissionTypeId] = useState('');
  const [date, setDate] = useState('');
  const [types, setTypes] = useState<PermissionType[]>([]);
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [loading, setLoading] = useState(false);
  const [loadingTypes, setLoadingTypes] = useState(true);

  const navigate = useNavigate();

  useEffect(() => {
    getAllPermissionTypes()
      .then((data) => setTypes(data))
      .catch(() => setErrorMessage('Error al cargar los tipos de permiso'))
      .finally(() => setLoadingTypes(false));
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setErrorMessage('');

    try {
      await axios.post('http://localhost:8080/api/Permissions', {
        employeeName: name,
        employeeLastName: surname,
        permissionTypeId: parseInt(permissionTypeId),
        permissionDate: date,
      });
      setOpenSnackbar(true);
      setTimeout(() => navigate('/'), 2000);
    } catch {
      setErrorMessage('Error al crear el permiso');
    } finally {
      setLoading(false);
    }
  };

  if (loadingTypes) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box display="flex" justifyContent="center" alignItems="center" height="100vh">
      <Paper elevation={3} sx={{ p: 4, width: '100%', maxWidth: 600 }}>
        <Typography variant="h5" gutterBottom align="center">
          Solicitar Permiso
        </Typography>

        <Box component="form" onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label="Nombre"
            value={name}
            onChange={(e) => setName(e.target.value)}
            margin="normal"
            required
          />
          <TextField
            fullWidth
            label="Apellido"
            value={surname}
            onChange={(e) => setSurname(e.target.value)}
            margin="normal"
            required
          />
          <TextField
            select
            fullWidth
            label="Tipo de Permiso"
            value={permissionTypeId}
            onChange={(e) => setPermissionTypeId(e.target.value)}
            margin="normal"
            required
          >
            {types.map((type) => (
              <MenuItem key={type.id} value={type.id}>
                {type.description}
              </MenuItem>
            ))}
          </TextField>
          <TextField
            fullWidth
            type="date"
            label="Fecha"
            InputLabelProps={{ shrink: true }}
            value={date}
            onChange={(e) => setDate(e.target.value)}
            margin="normal"
            required
          />

          <Button
            variant="contained"
            color="primary"
            type="submit"
            sx={{ mt: 2 }}
            fullWidth
            disabled={loading}
            startIcon={loading && <CircularProgress size={20} />}
          >
            {loading ? 'Enviando...' : 'Enviar'}
          </Button>
        </Box>

        <Snackbar open={openSnackbar} autoHideDuration={3000} onClose={() => setOpenSnackbar(false)}>
          <Alert severity="success" sx={{ width: '100%' }}>
            Permiso creado correctamente
          </Alert>
        </Snackbar>

        <Snackbar open={!!errorMessage} autoHideDuration={4000} onClose={() => setErrorMessage('')}>
          <Alert severity="error" sx={{ width: '100%' }}>
            {errorMessage}
          </Alert>
        </Snackbar>
      </Paper>
    </Box>
  );
};