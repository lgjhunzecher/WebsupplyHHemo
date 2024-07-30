﻿using Microsoft.Data.SqlClient;
using WebsupplyHHemo.API.Dto;

namespace WebsupplyHHemo.API.ADO
{
    public class RecebimentoADO
    {
        public string strMensagem = string.Empty;

        public bool ATUALIZA_RECEBIMENTO_FISCAL(string Connection, RecebimentoRequestDto objRequest)
        {
            // Seta os Parametros Iniciais de Retorno
            bool retorno = false;
            strMensagem = $"Erro durante a realização do Recebimento Fiscal. Parametros Enviados - NumDoc: [{objRequest.NumDoc}], NumPedido: [{objRequest.NumPedido}], Serie: [{objRequest.Serie}], CodFornecedor: [{objRequest.CodFornecedor}], CnpjFornecedor: [{objRequest.CnpjFornecedor}] e Status: [{objRequest.Status}]";

            ConexaoSQLServer Conn = new ConexaoSQLServer(Connection);

            string NomeProcedure = "SP_HHemo_WS_Recebimento_Fiscal_InsUPD";

            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@iNumPedido", objRequest.NumPedido));
            parametros.Add(new SqlParameter("@iNumNF", objRequest.NumDoc));
            parametros.Add(new SqlParameter("@vSERIE", objRequest.Serie));
            parametros.Add(new SqlParameter("@vCodFornecedor", objRequest.CodFornecedor));
            parametros.Add(new SqlParameter("@vCnpjFornecedor", objRequest.CnpjFornecedor));
            parametros.Add(new SqlParameter("@cStatus", objRequest.Status));

            using (var reader = Conn.ExecutaComParametros(NomeProcedure, parametros))
            {
                while (reader.HasRows)
                {
                    strMensagem = $"Recebimento Fiscal da Nota Nº [{objRequest.NumDoc}] referente ao pedido [{objRequest.NumPedido}] realizado com sucesso";
                    retorno = true;
                }
            }

            Conn.Dispose();

            return retorno;
        }
    }
}
