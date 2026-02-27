using FluentAssertions;
using Moq;
using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Test;

[TestClass]
public sealed class DispositivosHogarTest
{
    private Mock<IDispositivoRepositorio> _logicaDispositivoMock = null!;
    private Mock<IHogarLogica> _logicaHogarMock = null!;
    private Mock<IDispositivoHogarRepositorio> _logicaDispositivoHogarMock = null!;
    private DispositivoHogarLogica _servicio = null!;

    [TestInitialize]
    public void Initialize()
    {
        _logicaDispositivoMock = new Mock<IDispositivoRepositorio>(MockBehavior.Strict);
        _logicaHogarMock = new Mock<IHogarLogica>(MockBehavior.Strict);
        _logicaDispositivoHogarMock = new Mock<IDispositivoHogarRepositorio>(MockBehavior.Strict);
        _servicio = new DispositivoHogarLogica(_logicaDispositivoHogarMock.Object, _logicaDispositivoMock.Object, _logicaHogarMock.Object);
    }

    #region Error
    [TestMethod]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDispositivoHogarConDispositivoNullLanzaExcepcion(Dispositivo dispositivo)
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var args = new CrearDispositivosHogarArgs(
            dispositivo,
            new Hogar());

        _servicio.Agregar(args, usuario);
    }

    [TestMethod]
    [DataRow(null)]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearDispositivoHogarConHogarNullLanzaExcepcion(Hogar hogar)
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var args = new CrearDispositivosHogarArgs(
            new Dispositivo(),
            hogar);

        _servicio.Agregar(args, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearDispositivoHogarConDispositivoInexistenteLanzaExcepcion()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var args = new CrearDispositivosHogarArgs(
            new Dispositivo(),
            new Hogar());

        _logicaDispositivoHogarMock
            .Setup(i => i.Agregar(It.Is<DispositivoHogar>(d =>
                d.Id != Guid.Empty &&
                d.DispositivoId == args.Dispositivo.Id &&
                d.HogarId == args.Hogar.Id &&
                d.EstaConectado == args.EstaConectado)));

        _logicaDispositivoMock
            .Setup(i => i.Existe(d => d.Id == args.Dispositivo.Id))
            .Returns(false);

        _logicaHogarMock
            .Setup(i => i.Existe(h => h.Id == args.Hogar.Id))
            .Returns(true);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, args.Hogar.Id.ToString()))
            .Returns(true);

        _servicio.Agregar(args, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearDispositivoHogarConHogarInexistenteLanzaExcepcion()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var args = new CrearDispositivosHogarArgs(
            new Dispositivo(),
            new Hogar());

        _logicaDispositivoHogarMock
            .Setup(i => i.Agregar(It.Is<DispositivoHogar>(d =>
                d.Id != Guid.Empty &&
                d.DispositivoId == args.Dispositivo.Id &&
                d.HogarId == args.Hogar.Id &&
                d.EstaConectado == args.EstaConectado)));

        _logicaDispositivoMock
            .Setup(i => i.Existe(d => d.Id == args.Dispositivo.Id))
            .Returns(true);

        _logicaHogarMock
            .Setup(i => i.Existe(h => h.Id == args.Hogar.Id))
            .Returns(false);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, args.Hogar.Id.ToString()))
            .Returns(true);

        _servicio.Agregar(args, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ObtenerDispositivosDeHogarConIdInvalidoLanzaExcepcion()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var hogarIdInvalido = "IDInvalido";

        _servicio.ObtenerDispositivosDeHogar(hogarIdInvalido, usuario, null);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void ObtenerDispositivoHogarPorIdConHardwardIdInvalidoLanzaExcepcion()
    {
        var hardwardIdInvalido = "IDInvalido";

        _servicio.ObtenerDispositivoHogarPorId(hardwardIdInvalido);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void ObtenerDispositivoHogarPorIdConHardwardIdNoExistenteLanzaExcepcion()
    {
        var hardwardId = Guid.NewGuid().ToString();

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([]);

        _servicio.ObtenerDispositivoHogarPorId(hardwardId);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void EjecutarAccionNoSoportadaLanzaExcepcion()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.SensorVentana },
            HogarId = Guid.NewGuid(),
            EstaConectado = true,
            EstaAbierto = false
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id)));

        _servicio.EjecutarOperacionDispositivo(hardwardId, "Encender");
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CamaraDetectaPersonaConDispositivoNoCamaraLanzaExcepcion()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.SensorVentana },
            HogarId = Guid.NewGuid(),
            EstaConectado = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _servicio.CamaraDetectaPersona(hardwardId);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CamaraDetectaPersonaConCamaraSinFuncionamientoLanzaExcepcion()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo
            {
                Tipo = TipoDispositivo.Camara,
                DetectaPersona = false
            },
            HogarId = Guid.NewGuid(),
            EstaConectado = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _servicio.CamaraDetectaPersona(hardwardId);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CamaraDetectaMovimientoConDispositivoNoCamaraLanzaExcepcion()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.SensorVentana },
            HogarId = Guid.NewGuid(),
            EstaConectado = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _servicio.CamaraDetectaMovimiento(hardwardId);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CamaraDetectaMovimientoConCamaraSinFuncionamientoLanzaExcepcion()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo
            {
                Tipo = TipoDispositivo.Camara,
                DetectaMovimiento = false
            },
            HogarId = Guid.NewGuid(),
            EstaConectado = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _servicio.CamaraDetectaMovimiento(hardwardId);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void SensorDetectaMovimientoConDispositivoNoSensorMovimientoLanzaExcepcion()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.SensorVentana },
            HogarId = Guid.NewGuid(),
            EstaConectado = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _servicio.SensorDetectaMovimiento(hardwardId);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarDispositivoHogarAOtroCuartolanzaExcepcion()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var hogarId = Guid.NewGuid();

        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            Dispositivo = new Dispositivo
            {
                Tipo = TipoDispositivo.SensorVentana
            },
            HogarId = hogarId,
            EstaConectado = true,
            CuartoId = Guid.NewGuid()
        };

        var cuarto = new Cuarto
        {
            Id = Guid.NewGuid(),
            HogarId = hogarId
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _servicio.AgregarACuarto(dispositivoHogar.Id.ToString(), cuarto, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarDispositivoHogarACuartoDeOtroHogarlanzaExcepcion()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            Dispositivo = new Dispositivo
            {
                Tipo = TipoDispositivo.SensorVentana
            },
            HogarId = Guid.NewGuid(),
            EstaConectado = true,
            CuartoId = Guid.NewGuid()
        };

        var cuarto = new Cuarto
        {
            Id = Guid.NewGuid(),
            HogarId = Guid.NewGuid()
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _servicio.AgregarACuarto(dispositivoHogar.Id.ToString(), cuarto, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarDispositivoHogarACuartoSinPermisoLanzaExcepcion()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var hogarId = Guid.NewGuid();

        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            Dispositivo = new Dispositivo
            {
                Tipo = TipoDispositivo.SensorVentana
            },
            HogarId = hogarId,
            EstaConectado = true
        };

        var cuarto = new Cuarto
        {
            Id = Guid.NewGuid(),
            HogarId = hogarId
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AdministrarCuarto", usuario, cuarto.HogarId.ToString()))
            .Returns(false);

        _servicio.AgregarACuarto(dispositivoHogar.Id.ToString(), cuarto, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ConectarDispositivoHogarLanzaExcepcionSiNoTienePermiso()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            EstaConectado = false,
            HogarId = Guid.NewGuid()
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, dispositivoHogar.HogarId.ToString()))
            .Returns(false);

        _servicio.Conectar(dispositivoHogar.Id.ToString(), usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void DesconectarDispositivoHogarLanzaExcepcionSiNoTienePermiso()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            EstaConectado = true
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, dispositivoHogar.HogarId.ToString()))
            .Returns(false);

        _servicio.Desconectar(dispositivoHogar.Id.ToString(), usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ModificarNombreDispositivoHogarLanzaExcepcionSiNoTienePermiso()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var hogarId = Guid.NewGuid();
        var hardwardId = Guid.NewGuid();
        var nuevoNombre = "Porton Frente";

        var dispositivoHogar = new DispositivoHogar
        {
            Id = hardwardId,
            Nombre = "C21D",
            HogarId = hogarId
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("ModificarNombreDispositivo", usuario, dispositivoHogar.HogarId.ToString()))
            .Returns(false);

        _servicio.ActualizarNombreDispositivoHogar(hardwardId.ToString(), nuevoNombre, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CrearDispositivoHogarLanzaExcepcionSiNoTienePermiso()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var args = new CrearDispositivosHogarArgs(
            new Dispositivo
            {
                Id = Guid.NewGuid(),
            },
            new Hogar
            {
                Id = Guid.NewGuid()
            });

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, args.Hogar.Id.ToString()))
            .Returns(false);

        _servicio.Agregar(args, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ObtenerDispositivosHogarLanzaExcepcionSiNoTienePermiso()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var hogarId = Guid.NewGuid().ToString();
        var dispositivosHogar = new List<DispositivoHogar>
        {
            new DispositivoHogar { DispositivoId = Guid.NewGuid(), HogarId = Guid.Parse(hogarId), EstaConectado = true },
            new DispositivoHogar { DispositivoId = Guid.NewGuid(), HogarId = Guid.Parse(hogarId), EstaConectado = false }
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(null))
            .Returns(dispositivosHogar);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("ListarDispositivo", usuario, hogarId))
            .Returns(false);

        _servicio.ObtenerDispositivosDeHogar(hogarId, usuario, null);
    }
    #endregion

    #region Exito
    [TestMethod]
    public void CrearDispositivoHogarExito()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var args = new CrearDispositivosHogarArgs(
            new Dispositivo
            {
                Id = Guid.NewGuid(),
            },
            new Hogar
            {
                Id = Guid.NewGuid()
            });

        _logicaDispositivoMock
            .Setup(i => i.Existe(d => d.Id == args.Dispositivo.Id))
            .Returns(true);

        _logicaHogarMock
            .Setup(i => i.Existe(h => h.Id == args.Hogar.Id))
            .Returns(true);

        _logicaDispositivoHogarMock
            .Setup(i => i.Agregar(It.Is<DispositivoHogar>(d =>
                d.Id != Guid.Empty &&
                d.DispositivoId == args.Dispositivo.Id &&
                d.HogarId == args.Hogar.Id &&
                d.EstaConectado == args.EstaConectado)))
            .Returns((DispositivoHogar dispositivoHogar) => dispositivoHogar);

        _logicaDispositivoHogarMock
            .Setup(i => i.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, args.Hogar.Id.ToString()))
            .Returns(true);

        var respuesta = _servicio.Agregar(args, usuario);
        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.DispositivoId.Should().Be(args.Dispositivo.Id);
        respuesta.HogarId.Should().Be(args.Hogar.Id);
        respuesta.EstaConectado.Should().BeTrue();
    }

    [TestMethod]
    public void CrearDispositivoHogarTipoSensorVentanaExito()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var args = new CrearDispositivosHogarArgs(
            new Dispositivo
            {
                Id = Guid.NewGuid(),
                Tipo = TipoDispositivo.SensorVentana
            },
            new Hogar
            {
                Id = Guid.NewGuid()
            });

        _logicaDispositivoMock
            .Setup(i => i.Existe(d => d.Id == args.Dispositivo.Id))
            .Returns(true);

        _logicaHogarMock
            .Setup(i => i.Existe(h => h.Id == args.Hogar.Id))
            .Returns(true);

        _logicaDispositivoHogarMock
            .Setup(i => i.Agregar(It.Is<DispositivoHogar>(d =>
                d.Id != Guid.Empty &&
                d.DispositivoId == args.Dispositivo.Id &&
                d.HogarId == args.Hogar.Id &&
                d.EstaConectado == args.EstaConectado)))
            .Returns((DispositivoHogar dispositivoHogar) => dispositivoHogar);

        _logicaDispositivoHogarMock
            .Setup(i => i.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, args.Hogar.Id.ToString()))
            .Returns(true);

        var respuesta = _servicio.Agregar(args, usuario);
        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.DispositivoId.Should().Be(args.Dispositivo.Id);
        respuesta.HogarId.Should().Be(args.Hogar.Id);
        respuesta.EstaConectado.Should().BeTrue();
        respuesta.EstaAbierto.Should().BeFalse();
    }

    [TestMethod]
    public void CrearDispositivoHogarTipoLamparaExito()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var args = new CrearDispositivosHogarArgs(
            new Dispositivo
            {
                Id = Guid.NewGuid(),
                Tipo = TipoDispositivo.Lampara
            },
            new Hogar
            {
                Id = Guid.NewGuid()
            });

        _logicaDispositivoMock
            .Setup(i => i.Existe(d => d.Id == args.Dispositivo.Id))
            .Returns(true);

        _logicaHogarMock
            .Setup(i => i.Existe(h => h.Id == args.Hogar.Id))
            .Returns(true);

        _logicaDispositivoHogarMock
            .Setup(i => i.Agregar(It.Is<DispositivoHogar>(d =>
                d.Id != Guid.Empty &&
                d.DispositivoId == args.Dispositivo.Id &&
                d.HogarId == args.Hogar.Id &&
                d.EstaConectado == args.EstaConectado)))
            .Returns((DispositivoHogar dispositivoHogar) => dispositivoHogar);

        _logicaDispositivoHogarMock
            .Setup(i => i.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, args.Hogar.Id.ToString()))
            .Returns(true);

        var respuesta = _servicio.Agregar(args, usuario);
        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.DispositivoId.Should().Be(args.Dispositivo.Id);
        respuesta.HogarId.Should().Be(args.Hogar.Id);
        respuesta.EstaConectado.Should().BeTrue();
        respuesta.EstaAbierto.Should().BeNull();
        respuesta.EstaEncendida.Should().BeFalse();
    }

    [TestMethod]
    public void CrearDispositivoHogarTipoCamaraExito()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var args = new CrearDispositivosHogarArgs(
            new Dispositivo
            {
                Id = Guid.NewGuid(),
                Tipo = TipoDispositivo.Camara
            },
            new Hogar
            {
                Id = Guid.NewGuid()
            });

        _logicaDispositivoMock
            .Setup(i => i.Existe(d => d.Id == args.Dispositivo.Id))
            .Returns(true);

        _logicaHogarMock
            .Setup(i => i.Existe(h => h.Id == args.Hogar.Id))
            .Returns(true);

        _logicaDispositivoHogarMock
            .Setup(i => i.Agregar(It.Is<DispositivoHogar>(d =>
                d.Id != Guid.Empty &&
                d.DispositivoId == args.Dispositivo.Id &&
                d.HogarId == args.Hogar.Id &&
                d.EstaConectado == args.EstaConectado)))
            .Returns((DispositivoHogar dispositivoHogar) => dispositivoHogar);

        _logicaDispositivoHogarMock
            .Setup(i => i.GuardarCambios());

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, args.Hogar.Id.ToString()))
            .Returns(true);

        var respuesta = _servicio.Agregar(args, usuario);
        _servicio.GuardarCambios();

        respuesta.Should().NotBeNull();
        respuesta.Id.Should().NotBeEmpty();
        respuesta.Id.Should().NotBe(Guid.Empty);

        respuesta.DispositivoId.Should().Be(args.Dispositivo.Id);
        respuesta.HogarId.Should().Be(args.Hogar.Id);
        respuesta.EstaConectado.Should().BeTrue();
        respuesta.EstaAbierto.Should().BeNull();
        respuesta.EstaEncendida.Should().BeNull();
    }

    [TestMethod]
    public void ObtenerDispositivosDeHogarConIdValidoDevuelveDispositivos()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var hogarId = Guid.NewGuid().ToString();
        var dispositivosHogar = new List<DispositivoHogar>
        {
            new DispositivoHogar { DispositivoId = Guid.NewGuid(), HogarId = Guid.Parse(hogarId), EstaConectado = true },
            new DispositivoHogar { DispositivoId = Guid.NewGuid(), HogarId = Guid.Parse(hogarId), EstaConectado = false }
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(null))
            .Returns(dispositivosHogar);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("ListarDispositivo", usuario, hogarId))
            .Returns(true);

        var resultado = _servicio.ObtenerDispositivosDeHogar(hogarId, usuario, null);

        resultado.Should().NotBeNull();
        resultado.Count.Should().Be(2);
        resultado.All(d => d.HogarId == Guid.Parse(hogarId)).Should().BeTrue();
    }

    [TestMethod]
    public void ObtenerDispositivoHogarPorIdConHardwardIdValidoDevuelveDispositivo()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar { Id = Guid.Parse(hardwardId), DispositivoId = Guid.NewGuid(), HogarId = Guid.NewGuid(), EstaConectado = true };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        var resultado = _servicio.ObtenerDispositivoHogarPorId(hardwardId);

        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(Guid.Parse(hardwardId));
    }

    [TestMethod]
    public void SensorVentanaAbreEjecutaAccionCorrectamente()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.SensorVentana },
            HogarId = Guid.NewGuid(),
            EstaConectado = true,
            EstaAbierto = false
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id)));

        _servicio.EjecutarOperacionDispositivo(hardwardId, "Abre");

        dispositivoHogar.EstaAbierto.Should().BeTrue();
    }

    [TestMethod]
    public void SensorVentanaCierraEjecutaAccionCorrectamente()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.SensorVentana },
            HogarId = Guid.NewGuid(),
            EstaConectado = true,
            EstaAbierto = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id)));

        _servicio.EjecutarOperacionDispositivo(hardwardId, "Cierra");

        dispositivoHogar.EstaAbierto.Should().BeFalse();
    }

    [TestMethod]
    public void LamparaEnciendeEjecutaAccionCorrectamente()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.Lampara },
            HogarId = Guid.NewGuid(),
            EstaConectado = true,
            EstaEncendida = false
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id)));

        _servicio.EjecutarOperacionDispositivo(hardwardId, "Encender");

        dispositivoHogar.EstaEncendida.Should().BeTrue();
    }

    [TestMethod]
    public void LamparaApagaEjecutaAccionCorrectamente()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.Lampara },
            HogarId = Guid.NewGuid(),
            EstaConectado = true,
            EstaEncendida = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id)));

        _servicio.EjecutarOperacionDispositivo(hardwardId, "Apagar");

        dispositivoHogar.EstaEncendida.Should().BeFalse();
    }

    [TestMethod]
    public void ConectarDispositivoHogarActualizaEstadoYLlamaActualizarEnRepositorio()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            EstaConectado = false,
            HogarId = Guid.NewGuid()
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, dispositivoHogar.HogarId.ToString()))
            .Returns(true);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.IsAny<DispositivoHogar>()));

        _servicio.Conectar(dispositivoHogar.Id.ToString(), usuario);

        dispositivoHogar.EstaConectado.Should().BeTrue();
    }

    [TestMethod]
    public void DesconectarDispositivoHogarActualizaEstadoYLlamaActualizarEnRepositorio()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            EstaConectado = true
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AsociarDispositivo", usuario, dispositivoHogar.HogarId.ToString()))
            .Returns(true);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.IsAny<DispositivoHogar>()));

        _servicio.Desconectar(dispositivoHogar.Id.ToString(), usuario);

        dispositivoHogar.EstaConectado.Should().BeFalse();
    }

    [TestMethod]
    public void SensorDetectaMovimientoConDispositivoSensorMovimientoEjecutaCorrectamente()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo
            {
                Tipo = TipoDispositivo.SensorMovimiento
            },
            HogarId = Guid.NewGuid(),
            EstaConectado = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        Action act = () => _servicio.SensorDetectaMovimiento(hardwardId);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void CamaraDetectaMovimientoConDispositivoCamaraEjecutaCorrectamente()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo
            {
                Tipo = TipoDispositivo.Camara,
                DetectaMovimiento = true
            },
            HogarId = Guid.NewGuid(),
            EstaConectado = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        Action act = () => _servicio.CamaraDetectaMovimiento(hardwardId);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void CamaraDetectaPersonaConDispositivoCamaraEjecutaCorrectamente()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo
            {
                Tipo = TipoDispositivo.Camara,
                DetectaPersona = true
            },
            HogarId = Guid.NewGuid(),
            EstaConectado = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        Action act = () => _servicio.CamaraDetectaPersona(hardwardId);

        act.Should().NotThrow();
    }

    [TestMethod]
    public void SensorVentanaAbreRetornaFalseSiYaEstaAbierto()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.SensorVentana },
            HogarId = Guid.NewGuid(),
            EstaConectado = true,
            EstaAbierto = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id)));

        var estabaCerrado = _servicio.EjecutarOperacionDispositivo(hardwardId, "Abre");

        estabaCerrado.Should().BeFalse();
    }

    [TestMethod]
    public void SensorVentanaCierraRetornaFalseSiYaEstaCerrado()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.SensorVentana },
            HogarId = Guid.NewGuid(),
            EstaConectado = true,
            EstaAbierto = false
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id)));

        var estabaAbierto = _servicio.EjecutarOperacionDispositivo(hardwardId, "Cierra");

        estabaAbierto.Should().BeFalse();
    }

    [TestMethod]
    public void LamparaEnciendeRetornaFalseSiYaEstaEncendida()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.Lampara },
            HogarId = Guid.NewGuid(),
            EstaConectado = true,
            EstaEncendida = true
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id)));

        var estabaApagada = _servicio.EjecutarOperacionDispositivo(hardwardId, "Encender");

        estabaApagada.Should().BeFalse();
    }

    [TestMethod]
    public void LamparaApagaRetornaFalseSiYaEstaApagada()
    {
        var hardwardId = Guid.NewGuid().ToString();
        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.Parse(hardwardId),
            Dispositivo = new Dispositivo { Tipo = TipoDispositivo.Lampara },
            HogarId = Guid.NewGuid(),
            EstaConectado = true,
            EstaEncendida = false
        };

        _logicaDispositivoHogarMock
            .Setup(repo => repo.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id)));

        var estabaEncendida = _servicio.EjecutarOperacionDispositivo(hardwardId, "Apagar");

        estabaEncendida.Should().BeFalse();
    }

    [TestMethod]
    public void AgregarDispositivoHogarAUnCuartoExito()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var hogarId = Guid.NewGuid();

        var dispositivoHogar = new DispositivoHogar
        {
            Id = Guid.NewGuid(),
            Dispositivo = new Dispositivo
            {
                Tipo = TipoDispositivo.SensorVentana
            },
            HogarId = hogarId,
            EstaConectado = true
        };

        var cuarto = new Cuarto
        {
            Id = Guid.NewGuid(),
            HogarId = hogarId
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("AdministrarCuarto", usuario, cuarto.HogarId.ToString()))
            .Returns(true);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id)));

        _servicio.AgregarACuarto(dispositivoHogar.Id.ToString(), cuarto, usuario);

        dispositivoHogar.CuartoId.Should().Be(cuarto.Id);
    }

    [TestMethod]
    public void ActualizarNombreDispositivoHogarExito()
    {
        var usuario = new Usuario
        {
            Nombre = "Coco",
            Apellido = "Perez",
            Email = "admin@gmail.com",
            Contraseña = "coco1234.",
            Rol = RolesPredefinidos.Admin,
            RolId = RolesPredefinidos.ID_DUEÑO_HOGAR,
            FotoPerfil = "/downloads/cocoPerez"
        };

        var hogarId = Guid.NewGuid();
        var hardwardId = Guid.NewGuid();
        var nuevoNombre = "Porton Frente";

        var dispositivoHogar = new DispositivoHogar
        {
            Id = hardwardId,
            Nombre = "C21D",
            HogarId = hogarId
        };

        _logicaDispositivoHogarMock
            .Setup(dh => dh.ObtenerTodos(It.IsAny<ParametroDispositivoHogarFiltro>()))
            .Returns([dispositivoHogar]);

        _logicaHogarMock
            .Setup(h => h.VerificarPermiso("ModificarNombreDispositivo", usuario, dispositivoHogar.HogarId.ToString()))
            .Returns(true);

        _logicaDispositivoHogarMock
            .Setup(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id && d.Nombre == nuevoNombre)));

        _servicio.ActualizarNombreDispositivoHogar(hardwardId.ToString(), nuevoNombre, usuario);

        dispositivoHogar.Nombre.Should().Be(nuevoNombre);
        _logicaDispositivoHogarMock.Verify(dh => dh.Actualizar(It.Is<DispositivoHogar>(d => d.Id == dispositivoHogar.Id && d.Nombre == nuevoNombre)), Times.Once);
    }
    #endregion
}
