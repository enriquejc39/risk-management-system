import { Routes, Route } from 'react-router-dom'
import { useIsAuthenticated } from '@azure/msal-react'
import Layout from './components/Layout'
import Dashboard from './pages/Dashboard'
import Questionnaire from './pages/Questionnaire'
import RiskMap from './pages/RiskMap'
import AuditView from './pages/AuditView'
import Login from './pages/Login'

function App() {
  const isAuthenticated = useIsAuthenticated()

  if (!isAuthenticated) {
    return <Login />
  }

  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/questionnaire" element={<Questionnaire />} />
        <Route path="/risk-map" element={<RiskMap />} />
        <Route path="/audit" element={<AuditView />} />
      </Routes>
    </Layout>
  )
}

export default App
