import { createContext, useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { login as loginApi } from '../pages/api';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const navigate = useNavigate();
  // Inicializamos el estado leyendo directamente del localStorage para evitar "parpadeos"
  const [user, setUser] = useState(() => {
    const token = localStorage.getItem('token');
    const email = localStorage.getItem('email');
    return token && email ? { email } : null;
  });

  const login = async (data) => {
    try {
      const response = await loginApi(data);
      if (response.token) {
        localStorage.setItem('token', response.token);
        localStorage.setItem('email', data.email);
        setUser({ email: data.email });
        navigate('/'); // Redirigir al home
        return { success: true };
      }
      return { success: false, message: 'No se recibió el token.' };
    } catch (error) {
      console.error("Login error:", error);
      // Intentamos mostrar el mensaje que viene del backend (ej: "Usuario no encontrado")
      const msg = error.response?.data || 'Credenciales inválidas o error de servidor.';
      return { success: false, message: typeof msg === 'string' ? msg : JSON.stringify(msg) };
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    setUser(null);
    navigate('/login');
  };

  return (
    <AuthContext.Provider value={{ user, login, logout, isAuthenticated: !!user }}>
      {children}
    </AuthContext.Provider>
  );
};
