﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SgiConnection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WebsupplyHHemo.InterfaceNew.Funcoes;
using WebsupplyHHemo.InterfaceNew.Model;

namespace WebsupplyHHemo.InterfaceNew.Metodos
{
    public class UnidadesFiliaisMetodo
    {
        static int _intNumTransacao = 0;
        static int _intNumServico = 1;
        string strIdentificador = "Und" + Mod_Gerais.RetornaIdentificador();

        public string strMensagem = string.Empty;


        private static int intNumTransacao
        {
            get
            {
                _intNumTransacao += 1;
                return _intNumTransacao;
            }
            set
            {
                _intNumTransacao = value;
            }
        }

        public async Task<bool> CadastraAtualiza(string CGC)
        {
            bool retorno = false;
            Class_Log_Hhemo objLog;

            try
            {
                // Cria o Cliente Http
                HttpClient cliente = new HttpClient();

                // Gera Log
                objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                 0, 0, "", null, "Chamada a API Rest - Método " + Mod_Gerais.MethodName(),
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Parametros para Controle de Paginação
                int totalRegistros = 0;
                int linhaInicial = 0;
                int limiteRegistrosPagina = 100;
                int totalRegistrosPagina = 0;

                // Pega a URL do Serviço
                Class_Servico objServico = new Class_Servico();
                if (objServico.CarregaDados(_intNumServico, "", strIdentificador, intNumTransacao) == false)
                {
                    objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                                       1, -1, "", null, "Erro ao recuperar dados do serviço",
                                                       "", "", "", Mod_Gerais.MethodName());
                    objLog.GravaLog();
                    objLog = null;
                    strMensagem = "Erro ao recuperar dados do serviço";
                    return false;
                }
                else
                { _intNumTransacao -= 1; }

                // Cria um laço para percorrer todas as linhas
                do
                {
                    // Define a Estrutura da Request
                    object requestBody = new
                    {
                        tokenid = "HH@2021!%",
                        M0_CODIGO = "01",
                        ROWINI = linhaInicial,
                        ROWLINES = limiteRegistrosPagina
                    };

                    // Serializa o objeto para JSON
                    string jsonRequestBody = JsonConvert.SerializeObject(requestBody);

                    // Adiciona o JSON como conteúdo da requisição
                    var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                    // Define os parâmetros e cria a chamada
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(objServico.strURL),
                        Content = content
                    };

                    // Envia a requisição
                    var response = await cliente.SendAsync(request).ConfigureAwait(false);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    // Trata o Retorno e aloca no objeto
                    JArray retornoAPI = JArray.Parse(responseBody);

                    // Verifica se tem retorno
                    if (retornoAPI.Count > 0)
                    {
                        // Realiza a Chamada do Banco
                        Conexao conn = new Conexao(Mod_Gerais.ConnectionString());

                        // Percorre Todos os Resultados
                        for (int i = 0; i < retornoAPI.Count; i++)
                        {
                            // Pega a Linha do Retorno
                            JObject linhaRetorno = JObject.Parse(retornoAPI[i].ToString());

                            // Sincroniza o Retorno da API com a Classe de Gerenciamento
                            UnidadeModel unidade = new UnidadeModel
                            {
                                CGC = CGC,
                                CodUnidade = linhaRetorno["M0_CODFIL"].ToString().Trim(),
                                NomeCompleto = linhaRetorno["M0_NOMECOM"].ToString().Trim(),
                                Cidade = linhaRetorno["M0_CIDENT"].ToString().Trim(),
                                IE = linhaRetorno["M0_INSC"].ToString().Trim(),
                                Bairro = linhaRetorno["M0_BAIRENT"].ToString().Trim(),
                                Endereco = linhaRetorno["M0_ENDENT"].ToString().Trim(),
                                Complemento = linhaRetorno["M0_COMPENT"].ToString().Trim(),
                                CEP = linhaRetorno["M0_CEPENT"].ToString().Trim(),
                                UF = linhaRetorno["M0_ESTENT"].ToString().Trim(),
                                CNPJ = linhaRetorno["M0_CGC"].ToString().Trim(),
                                CodMunicipio = linhaRetorno["M0_CODMUN"].ToString().Trim(),
                                Status = linhaRetorno["M0_STATUS"].ToString().Trim(),
                            };

                            // Cria o Parametro da query do banco

                            ArrayList arrParam = new ArrayList();

                            arrParam.Add(new Parametro("@cCGC", unidade.CGC == "" ? null : unidade.CGC.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vCodEmpresa", unidade.CodUnidade == "" ? null : unidade.CodUnidade.ToString(), SqlDbType.VarChar, 500, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vDescEmpresa", unidade.NomeCompleto == "" ? null : unidade.NomeCompleto.ToString(), SqlDbType.VarChar, 2000, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vCidade", unidade.Cidade == "" ? null : unidade.Cidade.ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vIE", unidade.IE == "" ? null : unidade.IE.ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vBairro", unidade.Bairro == "" ? null : unidade.Bairro.ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vEndereco", unidade.Endereco == "" ? null : unidade.Endereco.ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vComplemento", unidade.Complemento == "" ? null : unidade.Complemento.ToString(), SqlDbType.VarChar, 100, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vCEP", unidade.CEP == "" ? null : unidade.CEP.ToString(), SqlDbType.VarChar, 10, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@cUF", unidade.UF == "" ? null : unidade.UF.ToString(), SqlDbType.VarChar, 3, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@CNPJ", unidade.CNPJ == "" ? null : unidade.CNPJ.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@vCodIBGE", unidade.CodMunicipio == "" ? null : unidade.CodMunicipio.ToString(), SqlDbType.VarChar, 15, ParameterDirection.Input));
                            arrParam.Add(new Parametro("@cStatus", unidade.Status == "1" ? "S" : "N", SqlDbType.VarChar, 15, ParameterDirection.Input));

                            ArrayList arrOut = new ArrayList();

                            conn.ExecuteStoredProcedure(new StoredProcedure("SP_HHEMO_EMPRESAS_INSUPD", arrParam), ref arrOut);
                        }

                        // Encerra a Conexão com Banco de Dados
                        conn.Dispose();

                        // Registra o Total de Registros da Pagina
                        totalRegistrosPagina = retornoAPI.Count;

                        // Seta o Total de Registros
                        totalRegistros += totalRegistrosPagina;

                        // Atualiza a Paginação
                        linhaInicial = totalRegistros + 1;
                    }
                } while (totalRegistrosPagina == limiteRegistrosPagina);

                // Retorna a Mensagem de Sucesso
                if (totalRegistros > 0)
                {
                    strMensagem = $"{totalRegistros} Unidade(s)/Filiai(s) cadastrada(s)/atualizada(s) com sucesso";
                }
                else
                {
                    strMensagem = "Requisição concluída com sucesso sem dados retornados";
                }

                // Gera Log
                objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                 0, 0, "", null, strMensagem,
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                return true;
            }
            catch (Exception ex)
            {
                // Estrutura o Erro
                strMensagem = ex.Message;

                // Gera Log
                objLog = new Class_Log_Hhemo(strIdentificador, intNumTransacao, _intNumServico,
                                 1, -1, "", null, strMensagem,
                                 "L", "", "", Mod_Gerais.MethodName());
                objLog.GravaLog();
                objLog = null;

                // Retorna Falso
                return false;
            }
        }
    }
}
