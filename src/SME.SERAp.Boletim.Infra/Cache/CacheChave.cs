﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Cache
{
    public static class CacheChave
    {
        /// <summary>
        /// Questões resumidas da prova do aluno
        /// 0 - Prova Id
        /// </summary>
        public const string QuestaoProvaAlunoResumo = "pqr-{0}-{1}";
        /// <summary>
        /// Questões resumidas da prova
        /// 0 - Prova Id
        /// </summary>
        public const string QuestaoProvaResumo = "pqr-{0}";
        /// <summary>
        /// Contextos resumidos da prova
        /// 0 - Prova Id
        /// </summary>
        public const string ContextoProvaResumo = "pcr-{0}";
        /// <summary>
        /// Caderno do aluno na prova
        /// 0 - Prova Id
        /// 1 - RA do aluno
        /// </summary>
        public const string AlunoCadernoProva = "alcp-{0}-{1}";
        /// <summary>
        /// Aluno
        /// 0 - RA do aluno
        /// </summary>
        public const string Aluno = "al-{0}";
        /// <summary>
        /// Questão completa
        /// 0 - Questão id
        /// </summary>
        public const string QuestaoCompleta = "qc-{0}";
        /// <summary>
        /// Questão completa id legado
        /// 0 - Questão id
        /// </summary>
        public const string QuestaoCompletaLegado = "qcl-{0}";
        /// <summary>
        /// Prova
        /// 0 - Prova Id
        /// </summary>
        public const string Prova = "p-{0}";
        /// <summary>
        /// Parametros
        /// </summary>
        public const string Parametros = "parametros";
        /// <summary>
        /// Provas
        /// </summary>
        public const string ProvasAnosDatasEModalidades = "pas";
        /// <summary>
        /// Dados do aluno logado
        /// 0 - RA do aluno
        /// </summary>
        public const string MeusDados = "ra-{0}";
        /// <summary>
        /// Provas Anterior por aluno
        /// {0} - RA do aluno
        /// </summary>
        public const string ProvasAnteriorAluno = "paf-{0}";
        /// <summary>
        /// Preferencias do aluno
        /// 0 - RA do aluno
        /// </summary>
        public const string PreferenciasAluno = "prefa-{0}";
        /// <summary>
        /// Exportacao resultado prova
        /// 0 - exportacao resultado id
        /// 1 - prova serap id
        /// </summary>
        public const string ExportacaoResultado = "exportacao-{0}-prova-{1}";
        /// <summary>
        /// Status Exportacao resultado prova
        /// 0 - exportacao resultado id
        /// 1 - prova serap id
        /// </summary>
        public const string ExportacaoResultadoStatus = "exportacao-{0}-prova-{1}-status";
        /// <summary>
        /// Versão do Frontend
        /// </summary>
        public const string VersaoFront = "versao-front";
        /// <summary>
        /// Versão da Api
        /// </summary>
        public const string VersaoApi = "versao-api";
        /// <summary>
        /// Código para autenticação do acesso administrador
        /// 0 - Código
        /// </summary>
        public const string CodigoAutenticacaoAdmin = "auth-adm-{0}";
        /// <summary>
        /// Turmas do aluno
        /// 0 - Código ra do aluno
        /// </summary>
        public const string AlunoTurma = "al-turmas-{0}";
        /// <summary>
        /// Prova do aluno
        /// 0 - Código da prova
        /// 1 - Código ra do aluno
        /// </summary>
        public const string AlunoProva = "al-prova-{0}-{1}";

        /// <summary>
        /// Questões Amostra Tai Aluno
        /// 0 - Código ra do aluno
        /// 1 - Código da Prova
        /// </summary>
        public const string QuestaoAmostraTaiAluno = "al-q-tai-prova-{0}-{1}";

        /// <summary>
        /// Resposta Amostra Tai Aluno
        /// 0 - Código ra do aluno
        /// 1 - Código da Prova
        /// </summary>
        public const string RespostaAmostraTaiAluno = "al-q-tai-prova-resposta-{0}-{1}";

        /// <summary>
        /// 0 - prova tai id
        /// 1 - aluno id
        /// </summary>
        public const string SincronizandoProvaTaiAluno = "sinc-prova-tai-aluno-{0}-{1}";

        /// <summary>
        /// 0 - Login do usuário
        /// </summary>
        public const string AtribuicoesEolUsuario = "atribuicoes-eol-usuario-{0}";

        /// <summary>
        /// 0 - RF do usuário
        /// </summary>
        public const string UeDreAtribuidasEolUsuario = "ue-dre-atribuidas-eol-usuario-{0}";
    }
}