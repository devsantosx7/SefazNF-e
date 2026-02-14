# 📄 SefazNF-e Monitor

> Aplicativo desktop (Windows Forms) para **buscar**, **baixar** e **enviar NF-e por e-mail** com agilidade e controle operacional.

![Status](https://img.shields.io/badge/status-em%20uso-2ea44f)
![Plataforma](https://img.shields.io/badge/plataforma-Windows-0078D6)
![.NET](https://img.shields.io/badge/.NET-net452-512BD4)
![Licença](https://img.shields.io/badge/licen%C3%A7a-MIT-blue)

---<img width="1094" height="700" alt="image" src="https://github.com/user-attachments/assets/e29ba1ba-1d65-409c-8d05-aaeb51b56539" />


## ✨ Visão geral

O **SefazNF-e Monitor** centraliza a rotina fiscal em uma interface única para times que precisam lidar com alto volume de documentos eletrônicos.

Com ele, você consegue:

- 🔎 **Buscar documentos fiscais** por período, termo e empresa/filial.
- ⬇️ **Baixar arquivos fiscais** em XML/PDF (inclusive em lote).
- 📧 **Enviar NF-e por e-mail** para parceiros e/ou para cópia própria.
- 🧩 Organizar eventos e documentos com filtros e abas especializadas.

---

## 🚀 Principais funcionalidades

### 1) Busca inteligente de documentos
- Pesquisa por chave/termo diretamente no monitor.
- Filtros por intervalo de datas e por filial.
- Navegação por abas para diferentes tipos de documentos.

### 2) Download facilitado
- Download de XML e PDF individual ou em lote.
- Opção de compactação (ZIP) para compartilhamento e arquivamento.
- Fluxo otimizado para reduzir tarefas manuais repetitivas.

### 3) Envio por e-mail integrado
- Envio para destinatários parceiros (campo de e-mail dedicado).
- Opção de envio para o próprio usuário.
- Personalização de assunto e preferências do envio.
- Interface voltada para operação rápida com seleção de documentos.

### 4) Operação para múltiplas filiais
- Gestão e troca de contexto por filial na mesma aplicação.
- Estrutura com painéis e controles para operação centralizada.

---

## 🖥️ Público-alvo

Este app é ideal para:

- Escritórios de contabilidade;
- Times fiscais de indústrias, varejo e distribuição;
- Backoffice que precisa consultar NF-e e compartilhar rapidamente com clientes/fornecedores.

---

## 🧱 Stack e arquitetura técnica

- **Aplicação desktop:** Windows Forms
- **Framework:** .NET Framework 4.5.2 (`net452`)
- **Linguagem:** C#
- **Estrutura da solução:** `SefazNF-e.sln` com projeto principal `Monitor.csproj`
- **Dependências internas:** bibliotecas `*.fiscal.io` (serviços, dados, utilitários e plugins)

> ℹ️ O projeto utiliza DLLs de suporte local (`Program Files (x86)/Fiscal.io/MonitorDFe`).

---

## 📂 Estrutura resumida do repositório

```text
SefazNF-e/
├─ monitor/                     # Telas e lógica principal da aplicação
├─ Monitor.Commands/            # Comandos internos e inicialização
├─ Monitor.PanelManager/        # Painéis e banners de apoio
├─ Monitor.TabData.*            # Abas e visões específicas de documentos
├─ Monitor.csproj               # Projeto principal (Windows Forms / net452)
└─ SefazNF-e.sln                # Solução
```

---

## 🛠️ Como executar localmente

### Pré-requisitos

- Windows com .NET Framework 4.5.2
- Visual Studio com suporte a projetos Windows Forms
- DLLs internas esperadas pelo projeto na pasta padrão de instalação do MonitorDFe

### Passos

1. Abra a solução `SefazNF-e.sln` no Visual Studio.
2. Verifique se as referências externas (`*.fiscal.io` e demais DLLs) estão resolvidas.
3. Compile em **Debug** ou **Release**.
4. Execute o projeto `Monitor`.

---

## 📧 Fluxo sugerido: busca, download e envio de NF-e

1. **Selecionar filial** no monitor.
2. **Aplicar filtros de busca** (data, chave/termo e tipo de documento).
3. **Selecionar documentos** retornados na listagem.
4. Escolher entre:
   - **Baixar XML/PDF** (individual ou lote);
   - **Enviar por e-mail** com assunto e destinatários.
5. (Opcional) **Compactar em ZIP** antes do envio/download.

---

## ✅ Boas práticas de uso

- Mantenha o e-mail do usuário e destinatários sempre atualizados.
- Use filtros por período para reduzir tempo de consulta.
- Padronize o assunto dos e-mails para facilitar rastreabilidade.
- Utilize envio em lote com critério para evitar retrabalho operacional.

---

## 🔒 Segurança e conformidade

Por lidar com documentos fiscais e dados sensíveis:

- Restrinja acesso ao ambiente de execução.
- Garanta backup de XML/PDF e trilha de operação.
- Revise periodicamente permissões e credenciais.

---

## 📌 Roadmap sugerido

- [ ] Melhorar telemetria de envios e falhas por e-mail
- [ ] Exportação de relatórios operacionais
- [ ] Templates de assunto/mensagem por tipo de parceiro
- [ ] Histórico detalhado de ações por usuário

---

## 🤝 Contribuição

Contribuições são bem-vindas para melhoria de usabilidade, robustez e automações operacionais.

Se for contribuir:

1. Crie uma branch de feature;
2. Faça alterações objetivas e documentadas;
3. Abra um PR com contexto funcional e técnico.

---

## 📄 Licença

Este repositório está sob licença **MIT**. Consulte o arquivo `LICENSE`.

---

## 💬 Resumo em uma frase

**Um monitor fiscal completo para buscar, baixar e enviar NF-e por e-mail com produtividade e organização em múltiplas filiais.**
