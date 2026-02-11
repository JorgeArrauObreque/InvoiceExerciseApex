export default function ExampleCard() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-slate-100 p-4">
      <div className="group relative w-full max-w-sm overflow-hidden rounded-2xl bg-white shadow-xl transition-all hover:shadow-2xl">
        <div className="h-32 w-full bg-gradient-to-r from-cyan-500 to-blue-500"></div>
        <div className="relative px-6 pb-6 pt-2">
          <div className="-mt-12 mb-4 flex justify-center">
            <div className="flex h-20 w-20 items-center justify-center rounded-full border-4 border-white bg-white shadow-md text-4xl">
              🎨
            </div>
          </div>
          <div className="text-center">
            <h3 className="text-xl font-bold text-slate-800">Tailwind CSS v4</h3>
            <p className="mt-1 text-sm text-slate-500">Integración con Vite exitosa</p>
            <p className="mt-4 text-slate-600">
              Este es un componente de ejemplo estilizado completamente con clases de utilidad.
              ¡Pasa el mouse por encima para ver efectos!
            </p>
          </div>
          <div className="mt-6 flex justify-center gap-3">
            <button className="rounded-lg bg-slate-200 px-4 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-300">Documentación</button>
            <button className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white shadow-md transition-colors hover:bg-blue-700 hover:shadow-lg">Empezar</button>
          </div>
        </div>
      </div>
    </div>
  );
}