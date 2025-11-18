using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CapaPresentacion.Models
{
    public class ModificarCaja
    {
        public int item { get; set; }
        public string cod_empresa { get; set; }
        public string cod_sala { get; set; }
        public DateTime fecha_apertura { get; set; }
        public DateTime? fecha_cierre { get; set; }
        public int turno { get; set; }
        public int cod_caja { get; set; }
        public int cod_personal { get; set; }
        public int tipo_caja { get; set; }
        public double tipo_cambio { get; set; }
        public double compras { get; set; }
        public double billete_fallas { get; set; }
        public double monedas_fallas { get; set; }
        public double sobrantes { get; set; }
        public double faltantes { get; set; }
        public double vales { get; set; }
        public double vales_maquina { get; set; }
        public double premios { get; set; }
        public double otros { get; set; }
        public double monto_inicial_dinero { get; set; }
        public double saldo_precuadre { get; set; }
        public double total_saldo_inicial { get; set; }
        public double total_saldo_final { get; set; }
        public double total_saldo_ingresos { get; set; }
        public double total_saldo_salidas { get; set; }
        public double total_saldo_pagomanual { get; set; }
        public double saldo_tarjetas { get; set; }
        public double cod_registro_usuario { get; set; }
        public string caja_usuario { get; set; }
        public string caja_usuario_nro { get; set; }
        public string estado { get; set; }
        public string desc_caja { get; set; }
        public double monto_final_dinero { get; set; }
        public double? monto_reposicion_fondo_caja { get; set; }
        public double? monto_reposicion_caja_fondo { get; set; }
        public double monto_arrastre_deuda { get; set; }
        public double? monto_bar_cortesia { get; set; }
        public double? monto_bar_venta { get; set; }
        public string expediente { get; set; }
        public int B_10 { get; set; }
        public int B_20 { get; set; }
        public int B_50 { get; set; }
        public int B_100 { get; set; }
        public int B_200 { get; set; }
        public int? M_001 { get; set; }
        public int? M_005 { get; set; }
        public int M_01 { get; set; }
        public int M_02 { get; set; }
        public int M_05 { get; set; }
        public int M_1 { get; set; }
        public int M_2 { get; set; }
        public int M_5 { get; set; }
        public int bd_1 { get; set; }
        public int bd_5 { get; set; }
        public int BD_10 { get; set; }
        public int BD_20 { get; set; }
        public int BD_50 { get; set; }
        public int BD_100 { get; set; }
        public int BD_200 { get; set; }
        public double Pres_Sal { get; set; }
        public double Pres_Otros { get; set; }
        public double Devolucion_Sal { get; set; }
        public double Devolucion_Otros { get; set; }
        public double MontoOtros { get; set; }
        public string ValorSR { get; set; }
        public double Bingo1 { get; set; }
        public double Bingo2 { get; set; }
        public double Telefono1 { get; set; }
        public double? Telefono2 { get; set; }
        public double Otros1 { get; set; }
        public double Otros2 { get; set; }
        public string Observaciones { get; set; }
        public double monto_inicial_dinero_dolares { get; set; }
        public double monto_final_dinero_dolares { get; set; }
        public double monto_reposicion_fondo_caja_dolares { get; set; }
        public double monto_reposicion_caja_fondo_dolares { get; set; }
        public double D_PreCuadre { get; set; }
        public double D_PreCuadreDolares { get; set; }
        public double D_Asignados { get; set; }
        public double D_AsignadosDolares { get; set; }
        public double D_Dolares { get; set; }
        public double D_Otros_Ingresos { get; set; }
        public double D_Promocion { get; set; }
        public double D_Cortesia { get; set; }
        public double T_Vendidos { get; set; }
        public double T_Comprados { get; set; }
        public double T_Cortesia { get; set; }
        public double T_PagoManual { get; set; }
        public double T_Promocion { get; set; }
        public double monto_venta_normal { get; set; }
        public double monto_venta_tcredito { get; set; }
        public double monto_compras { get; set; }
        public double monto_pagosmanuales { get; set; }
        public double? monto_compras_maq_normal { get; set; }
        public double? monto_compras_maq_ingresomanual { get; set; }
    }
}