import {
  Box,
  Button,
  MenuItem,
  TextField,
  Typography,
  CircularProgress,
  Alert,
  Backdrop,
  Container,
} from '@mui/material';
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';
import type { PermissionType, PermissionDto } from '../types/permission';
import { modifyPermission } from '../services/permissionService';

const EditPermissionPage = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [name, setName] = useState('');
  const [surname, setSurname] = useState('');
  const [permissionTypeId, setPermissionTypeId] = useState('');
  const [date, setDate] = useState('');
  const [types, setTypes] = useState<PermissionType[]>([]);
  const [loadingData, setLoadingData] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    const fetchTypes = axios.get('http://localhost:8080/api/PermissionTypes');
    const fetchPermissions = axios.get('http://localhost:8080/api/Permissions');

    Promise.all([fetchTypes, fetchPermissions])
      .then(([typesRes, permissionsRes]) => {
        setTypes(typesRes.data.data);
        const allPermissions: PermissionDto[] = permissionsRes.data.data;
        const found = allPermissions.find((p) => p.id === parseInt(id || '', 10));
        if (!found) throw new Error('Permiso no encontrado');

        setName(found.employeeName);
        setSurname(found.employeeLastName);
        setPermissionTypeId(found.permissionTypeId.toString());
        setDate(found.permissionDate.split('T')[0]);
      })
      .catch(() => setError('Error al cargar los datos'))
      .finally(() => setLoadingData(false));
  }, [id]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);
    try {
      await modifyPermission(Number(id), {
        id: Number(id),
        employeeName: name,
        employeeLastName: surname,
        permissionTypeId: parseInt(permissionTypeId),
        permissionDate: date,
      } as PermissionDto);
      setSuccess(true);
      setTimeout(() => navigate('/'), 1500);
    } catch (err) {
      setError('Error al actualizar el permiso');
    } finally {
      setSubmitting(false);
    }
  };

  if (loadingData) {
    return (
      <Backdrop open>
        <CircularProgress color="inherit" />
      </Backdrop>
    );
  }

  return (
    <Container maxWidth="sm">
      <Box
        component="form"
        onSubmit={handleSubmit}
        sx={{
          mt: 8,
          display: 'flex',
          flexDirection: 'column',
          gap: 2,
          backgroundColor: '#fafafa',
          padding: 4,
          borderRadius: 2,
          boxShadow: 3,
        }}
      >
        <Typography variant="h5" gutterBottom>
          Editar Permiso
        </Typography>
        {error && <Alert severity="error">{error}</Alert>}
        {success && <Alert severity="success">Permiso actualizado correctamente</Alert>}

        <TextField
          label="Nombre"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
          disabled={submitting}
        />
        <TextField
          label="Apellido"
          value={surname}
          onChange={(e) => setSurname(e.target.value)}
          required
          disabled={submitting}
        />
        <TextField
          select
          label="Tipo de Permiso"
          value={permissionTypeId}
          onChange={(e) => setPermissionTypeId(e.target.value)}
          required
          disabled={submitting}
        >
          {types.map((type) => (
            <MenuItem key={type.id} value={type.id}>
              {type.description}
            </MenuItem>
          ))}
        </TextField>
        <TextField
          type="date"
          label="Fecha"
          InputLabelProps={{ shrink: true }}
          value={date}
          onChange={(e) => setDate(e.target.value)}
          required
          disabled={submitting}
        />
        <Button
          variant="contained"
          color="primary"
          type="submit"
          disabled={submitting}
          sx={{ mt: 2 }}
        >
          {submitting ? <CircularProgress size={24} color="inherit" /> : 'Actualizar'}
        </Button>
      </Box>
    </Container>
  );
};

export default EditPermissionPage;