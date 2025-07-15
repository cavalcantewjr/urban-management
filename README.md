# Urban Management – Gestão de Incidentes

> Plataforma para gestão de chamados ou incidentes, com foco em eficiência operacional, transparência e planejamento territorial.

---

## 📱 Objetivo

Facilitar o gerenciamento de chamados urbanos abertos por cidadãos ou colaboradores, permitindo que líderes de equipe distribuam os chamados aos responsáveis por áreas específicas e acompanhem todo o ciclo de atendimento — desde o recebimento até a resolução do problema pelo órgão ou administração competente.

---

## 👥 Perfis de Usuário

- **Cidadão:** Abre chamados (via app externo ou canal público futuro).
- **Líder:** Atribui chamados para membros do time e monitora métricas.
- **Integrante:** Atua no atendimento em campo, avalia, notifica órgãos e encerra chamados.

---

## 🛠️ Funcionalidades Principais

- Dashboard com mapa de calor dos chamados (Google Maps/Leaflet/Mapbox)
- Atribuição automática por CEP/bairro (API ViaCEP)
- Notificações em tempo real (SignalR)
- Relatórios de tempo de resposta e resolução
- Upload de fotos e anexos com validação de localização
- Painéis para líderes com métricas e indicadores de SLA

---

## 🌐 Tecnologias Sugeridas

- **Backend:** .Net 8 + REST API. 
- **Banco de dados e Serviços:** PostgreSQL + PostGIS, integração ViaCEP, SignalR.
- **Arquitetura e Padrão:** Clean Architecture e CQRS

---

## 🗺️ Módulos do Sistema

- Sendo elaborado. Em breve atualizações aqui. 🛠️
---

## 🚦 Status do Projeto

- [ ] Documentação inicial
- [ ] Estrutura básica do repositório
- [ ] Implementação backend
- [ ] Implementação frontend mobile
- [ ] Wireframes iniciais

---

## 📦 Como rodar localmente

```bash
# Clone o repositório
git clone https://github.com/sua-conta/urban-management.git
cd urban-management

# Leia os READMEs das pastas /backend
