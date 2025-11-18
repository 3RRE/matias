using CapaEntidad;
using CapaEntidad.Cortesias;
using CapaEntidad.Response;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CapaPresentacion.Utilitarios {

    public class ServiceCortesias {
        private readonly SalaBL salaBL;
        public readonly ServiceHelper _serviceHelper;
        private readonly bool develop = false;
        private readonly string UrlDevelop = "http://localhost:64118/";

        public ServiceCortesias() {
            salaBL = new SalaBL();
            _serviceHelper = new ServiceHelper();

        }

        public async Task<List<T>> GetList<T>(string url, int codSala) {
            List<T> data = new List<T>();
            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}":$"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        //item
                    };

                    if(tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            //item
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    if(result.success) {
                        data = JsonConvert.DeserializeObject<List<T>>(result.data);
                    }

                }

            } catch(Exception) {
                data = new List<T>();
            }
            return data;
        }
        public async Task<List<T>> GetList<T, I>(string url, int codSala, I item) {
            List<T> data = new List<T>();
            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    url = $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        item
                    };

                    if(tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            item,
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    if(result.success) {
                        data = JsonConvert.DeserializeObject<List<T>>(result.data);
                    }

                }

            } catch(Exception) {
                data = new List<T>();
            }
            return data;
        }
        public async Task<T> GetItemById<T>(string url, int codSala, int id) {
            T item = default(T);

            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}" : $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        id
                    };

                    if(tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            id,
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    if(result.success) {
                        item = JsonConvert.DeserializeObject<T>(result.data);
                    }

                }

            } catch(Exception) {
                item = default(T);
            }
            return item;
        }
        public async Task<bool> CreateOrUpdate<T>(string url, int codSala, T item) {
            bool success = false;

            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}" : $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        item
                    };

                    if(tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            item,
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    success = result.success;

                }

            } catch(Exception) {
                success = false;
            }
            return success;
        }
        public async Task<bool> Delete<T>(string url, int codSala, int id) {
            bool success = false;

            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}" : $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        id
                    };

                    if(tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            id,
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    success = result.success;

                }

            } catch(Exception) {
                success = false;
            }
            return success;
        }
        public async Task<bool> AsignarMaquinas(string url, int codSala, int id, List<string> maquinas) {
            bool success = false;

            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}" : $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        id,
                        maquinas
                    };

                    if(tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            id,
                            maquinas,
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    success = result.success;

                }

            } catch(Exception) {
                success = false;
            }
            return success;
        }
        public async Task<bool> AsignarAnfitrionas(string url, int codSala, int id, List<int> anfitrionas) {
            bool success = false;

            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}" : $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        id,
                        anfitrionas
                    };

                    if(tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            id,
                            anfitrionas,
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    success = result.success;

                }

            } catch(Exception) {
                success = false;
            }
            return success;
        }
        public async Task<List<T>> GetListById<T>(string url, int codSala,int id) {
            List<T> data = new List<T>();
            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}" : $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        id
                    };

                    if(tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            id,
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    if(result.success) {
                        data = JsonConvert.DeserializeObject<List<T>>(result.data);
                    }

                }

            } catch(Exception) {
                data = new List<T>();
            }
            return data;
        }
        public async Task<bool> UpdateKeyValueConfiguracion(string url, int codSala, string key, int value) {
            bool success = false;
            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}" : $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        key,
                        value
                    };

                    if(tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            key,
                            value,
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    if(result.success) {
                        success = true;
                    }

                }

            } catch(Exception) {
                success = false;
            }
            return success;
        }
        public async Task<bool> UpdateKeyStateConfiguracion(string url, int codSala, string key, bool state) {
            bool success = false;
            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}" : $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        key,
                        state
                    };

                    if(tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            key,
                            state,
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    if(result.success) {
                        success = true;
                    }

                }

            } catch(Exception) {
                success = false;
            }
            return success;
        }


        public async Task<bool> SendImage(string url, int codSala, string imagePath,string imageName) {
            bool success = false;

            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if(tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}" : $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    if(!System.IO.File.Exists(imagePath)) {
                        return false;
                    }

                    if(tcpConnection.IsVpn) {
                        return false;
                    }


                    ResultEntidad result = await _serviceHelper.PostFileAsync(url, imagePath, imageName);

                    success = result.success;

                }

            } catch(Exception) {
                success = false;
            }
            return success;
        }



        public async Task<bool> CreateAnfitriona(string url, int codSala, CRT_Empleado empleado) {
            bool success = false;
            try {

                SalaEntidad sala = salaBL.SalaListaIdJson(codSala);

                CheckPortHelper checkPortHelper = new CheckPortHelper();
                CheckPortHelper.TcpConnection tcpConnection = checkPortHelper.TcpUrlMultiple(sala.UrlSalaOnline, new List<string>());

                if (tcpConnection.IsOpen) {

                    //url = $"{sala.UrlSalaOnline}/Cortesias/{url}";
                    url = develop ? $"{UrlDevelop}Cortesias/{url}" : $"{sala.UrlSalaOnline}/Cortesias/{url}";

                    object parameters = new {
                        empleado
                    };

                    if (tcpConnection.IsVpn) {
                        url = $"{tcpConnection.Url}/servicio/VPNGenericoPost";

                        parameters = new {
                            empleado,
                            ipPrivada = $"{sala.IpPrivada}:{sala.PuertoWebOnline}/online/Cortesias/{url}"
                        };
                    }

                    string content = JsonConvert.SerializeObject(parameters);

                    ResultEntidad result = await _serviceHelper.PostAsync(url, content);

                    if (result.success) {
                        success = true;
                    }

                }

            }
            catch (Exception) {
                success = false;
            }
            return success;
        }


    }
}