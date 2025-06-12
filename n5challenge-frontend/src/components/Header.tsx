import { AppBar, Toolbar, Typography } from '@mui/material';

const Header = () => {
  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" component="div">
          N5 Challenge - Permisos
        </Typography>
      </Toolbar>
    </AppBar>
  );
};

export default Header;