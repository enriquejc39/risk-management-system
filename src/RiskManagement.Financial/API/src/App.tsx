import { Routes, Route } from 'react-router-dom'
import { useIsAuthenticated } from '@azure/msal-react'
import Layout from './components/Layout'
import Dashboard from './pages/Dashboard'
import Questionnaire from './pages/Questionnaire'
import RiskMap from './pages/RiskMap'
import AuditView from './pages/AuditView'
import Login from './pages/Login'
import RiskCatalog from './pages/RiskCatalog'
import Controls from './pages/Controls'
import ActionPlans from './pages/ActionPlans'
import Evidences from './pages/Evidences'
import History from './pages/History'

function App() {
  const isAuthenticated = useIsAuthenticated()

  if (!isAuthenticated) {
    return <Login />
  }

  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Dashboard />} />
        <Route path="/risk-catalog" element={<RiskCatalog />} />
        <Route path="/risk-map" element={<RiskMap />} />
        <Route path="/questionnaire" element={<Questionnaire />} />
        <Route path="/controls" element={<Controls />} />
        <Route path="/action-plans" element={<ActionPlans />} />
        <Route path="/audit" element={<AuditView />} />
        <Route path="/evidences" element={<Evidences />} />
        <Route path="/history" element={<History />} />
      </Routes>
    </Layout>
  )
}

export default App
