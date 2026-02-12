import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function RequireAuth({ children }) {
  const { user } = useAuth();
  const location = useLocation();

  if (!user) {
    // Redirigir al login, guardando la ubicación actual para volver después 
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return children;
}
