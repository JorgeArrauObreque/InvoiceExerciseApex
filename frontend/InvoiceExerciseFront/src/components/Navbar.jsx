import { useState } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function Navbar() {
  const [isOpen, setIsOpen] = useState(false);
  const { user, logout } = useAuth();

  if (!user) return null;

  return (
    <nav className="bg-slate-800 text-white shadow-lg">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          <div className="flex items-center">
            <Link to="/" className="flex-shrink-0 font-bold text-xl text-blue-400">
              InvoiceApp
            </Link>
            <div className="hidden md:block">
              <div className="ml-10 flex items-baseline space-x-4">
                <Link to="/" className="hover:bg-slate-700 px-3 py-2 rounded-md text-sm font-medium transition-colors">Inicio</Link>
                <Link to="/busqueda-numero" className="hover:bg-slate-700 px-3 py-2 rounded-md text-sm font-medium transition-colors">Buscar Nº</Link>
                <Link to="/busqueda-estado" className="hover:bg-slate-700 px-3 py-2 rounded-md text-sm font-medium transition-colors">Buscar Estado</Link>
                <Link to="/reportes" className="hover:bg-slate-700 px-3 py-2 rounded-md text-sm font-medium transition-colors">Reportes</Link>
                <Link to="/cargar-facturas" className="hover:bg-slate-700 px-3 py-2 rounded-md text-sm font-medium transition-colors">Importar</Link>
              </div>
            </div>
          </div>
          <div className="hidden md:block">
            <div className="ml-4 flex items-center md:ml-6">
              <div className="relative ml-3">
                <div>
                  <button
                    onClick={() => setIsOpen(!isOpen)}
                    className="max-w-xs bg-slate-800 rounded-full flex items-center text-sm focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-slate-800 focus:ring-white cursor-pointer"
                    id="user-menu-button"
                    aria-expanded="false"
                    aria-haspopup="true"
                  >
                    <span className="sr-only">Open user menu</span>
                    <span className="h-8 w-8 rounded-full bg-slate-500 flex items-center justify-center font-bold uppercase">
                      {user.email.charAt(0)}
                    </span>
                  </button>
                </div>
                {isOpen && (
                  <div
                    className="origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black/5 focus:outline-none z-10"
                    role="menu"
                    tabIndex="-1"
                  >
                    <div className="px-4 py-2 text-xs text-gray-500 border-b border-gray-100">Hola, {user.email}</div>
                    <Link to="#" className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100" role="menuitem">Perfil</Link>
                    <button onClick={logout} className="block w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100" role="menuitem">Cerrar Sesión</button>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </nav>
  );
}