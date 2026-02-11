import Navbar from './components/Navbar';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Facturas from './pages/Facturas';
import Reportes from './pages/Reportes';
import CrearNotaCredito from './pages/CrearNotaCredito';

function App() {
  return (
    <div className="min-h-screen bg-slate-100">
      <Navbar />

      <main className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
        <div className="px-4 py-6 sm:px-0">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/facturas" element={<Facturas />} />
            <Route path="/reportes" element={<Reportes />} />
            <Route path="/notas-credito/crear" element={<CrearNotaCredito />} />
            <Route path="*" element={<div className="text-center"><h2>404: Página no encontrada</h2></div>} />
          </Routes>
        </div>
      </main>
    </div>
  )
}

export default App
