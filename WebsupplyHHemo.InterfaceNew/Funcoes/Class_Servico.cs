﻿using SgiConnection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsupplyHHemo.InterfaceNew.Funcoes
{
    public class Class_Servico
    {
        #region Propriedades

        public string strURL { get; set; }

        #endregion

        #region Variáveis
        private ArrayList _ArrOut;
        private ArrayList _ArrParam;
        #endregion

        public bool CarregaDados(int _intIDServico,
                          string _strCodAmbiente,
                          string _strIdentificador,
                          int _intNumTransacao)
        {
            if (_strCodAmbiente == "")
            {
                string strNomeMaquina = Environment.MachineName;
                switch (strNomeMaquina.ToString().ToUpper())
                {
                    case "SRVWEB1":
                        _strCodAmbiente = "PRD";
                        break;
                    case "SRVWEB2":
                        _strCodAmbiente = "PRD";
                        break;
                    case "SRVWEBPREPRD":
                        _strCodAmbiente = "PRE";
                        break;
                    case "SRVHOMOLOG":
                        _strCodAmbiente = "HOM";
                        break;
                    default:
                        _strCodAmbiente = "DEV";
                        break;
                }
                // if (strNomeMaquina.ToString().ToUpper() == "SRVWEB1" ||
                //     strNomeMaquina.ToString().ToUpper() == "SRVWEB2")
                // {
                //     _strCodAmbiente = "PRD";
                // }
                // else
                // {
                //     _strCodAmbiente = "DEV";
                // }
                //// _strCodAmbiente = "PRD"; 
            }

            Conexao _Conn = new Conexao(Mod_Gerais.ConnectionString());
            try
            {
                _ArrParam = new ArrayList();
                _ArrParam.Add(new Parametro("@iIdServico", _intIDServico.ToString(), SqlDbType.Int, 4, ParameterDirection.Input));
                _ArrParam.Add(new Parametro("@cCodAmbiente", _strCodAmbiente.ToString(), SqlDbType.Char, 3, ParameterDirection.Input));

                _ArrOut = new ArrayList();
                DataTable _dtRetorno = _Conn.ExecuteStoredProcedure(new StoredProcedure("SP_WS_HHEMO_RECUPERA_ENDERECO", _ArrParam), ref _ArrOut).Tables[0];

                if (_dtRetorno != null)
                {
                    if (_dtRetorno.Rows.Count == 0)
                    {
                        throw new Exception("Não foi possível recuperar dados do serviço para os paramêtros intIDServico = " +
                                            _intIDServico.ToString() + " - _strCodAmbiente = " + _strCodAmbiente);
                    }
                    strURL = _dtRetorno.Rows[0]["URL"].ToString();
                }
                else
                {
                    throw new Exception("Não foi possível recuperar dados do serviço para os paramêtros intIDServico = " +
                                        _intIDServico.ToString() + " - _strCodAmbiente = " + _strCodAmbiente);
                }
                return true;
            }
            catch (Exception ex)
            {
                Class_Log_Hhemo _objLog = new Class_Log_Hhemo(_strIdentificador, _intNumTransacao, 0,
                                                                        0, -1, "", null,
                                                                        "Erro Classe : " + Mod_Gerais.MethodName() + " - Descr:" + ex.Message.ToString(),
                                                                        "", "", "", Mod_Gerais.MethodName());
                _objLog.GravaLog();
                _objLog = null;
                return false;
            }
            finally
            {
                if (_Conn != null)
                {
                    _Conn.Dispose();
                    _Conn = null;
                }
            }
        }
    }
}
