import Navbar from './components/Navbar';
import RequireAuth from './components/RequireAuth';
import { AuthProvider } from './context/AuthContext';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import BusquedaPorNumero from './pages/BusquedaPorNumero';
import BusquedaPorEstado from './pages/BusquedaPorEstado';
import Reportes from './pages/Reportes';
import CrearNotaCredito from './pages/CrearNotaCredito';
import CargarFacturas from './pages/CargarFacturas';
import Login from './pages/Login';

function App() {
  return (
    <AuthProvider>
      <div className="min-h-screen bg-slate-100">
        <Navbar />

        <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
          <div className="px-4 py-6 sm:px-0">
            <Routes>
              <Route path="/login" element={<Login />} />
              <Route path="/" element={<RequireAuth><Home /></RequireAuth>} />
              <Route path="/busqueda-numero" element={<RequireAuth><BusquedaPorNumero /></RequireAuth>} />
              <Route path="/busqueda-estado" element={<RequireAuth><BusquedaPorEstado /></RequireAuth>} />
              <Route path="/reportes" element={<RequireAuth><Reportes /></RequireAuth>} />
              <Route path="/notas-credito/crear" element={<RequireAuth><CrearNotaCredito /></RequireAuth>} />
              <Route path="/cargar-facturas" element={<RequireAuth><CargarFacturas /></RequireAuth>} />
              <Route path="*" element={<RequireAuth><div className="text-center"><h2>404: Página no encontrada</h2></div></RequireAuth>} />
            </Routes>
          </div>
        </main>
      </div>
    </AuthProvider>
  )
}

export default App
