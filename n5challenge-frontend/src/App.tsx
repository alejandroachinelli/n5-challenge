import { Route, Routes, Link } from 'react-router-dom';
import { Container, AppBar, Toolbar, Typography, Button } from '@mui/material';
import PermissionsPage from './pages/PermissionsPage';
import { PermissionForm } from './components/PermissionForm';
import EditPermissionPage from './pages/EditPermissionPage';

function App() {
  return (
    <>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            N5 Challenge - Permisos
          </Typography>
          <Button color="inherit" component={Link} to="/">Lista</Button>
          <Button color="inherit" component={Link} to="/create">Solicitar</Button>
        </Toolbar>
      </AppBar>

      <Container sx={{ mt: 4 }}>
        <Routes>
          <Route path="/" element={<PermissionsPage />} />
          <Route path="/create" element={<PermissionForm />} />
          <Route path="/edit/:id" element={<EditPermissionPage />} />
        </Routes>
      </Container>
    </>
  );
}

export default App;