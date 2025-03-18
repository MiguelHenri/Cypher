import { BrowserRouter, Route, Routes } from 'react-router'
import Home from './pages/Home'
import ProtectedRoute from './components/ProtectedRoute'
import Passwords from './pages/Passwords'
import Layout from './components/Layout'
import axios from 'axios'
import Signup from './pages/Signup'

if (import.meta.env.VITE_BACKEND_URL)
  axios.defaults.baseURL = import.meta.env.VITE_BACKEND_URL;
else
  console.error("No AXIOS Url for connection found!")

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<Layout />}>
          <Route index element={<Home />} />
          <Route path='signup' element={<Signup />} />
          <Route path='passwords' element={
            <ProtectedRoute>
              <Passwords />
            </ProtectedRoute>
          } />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App
