USE [master]
GO
IF DB_ID('Hades2SalaEscape') IS NULL
BEGIN
    CREATE DATABASE [Hades2SalaEscape]
END
GO
USE [Hades2SalaEscape]
GO
IF OBJECT_ID(N'[dbo].[Usuario]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Usuario](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [nombreUsuario] [varchar](50) NOT NULL,
        [Sala] [int] NOT NULL DEFAULT (1),
     CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO
IF OBJECT_ID(N'[dbo].[Usuarios]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[Usuarios];
END
GO
IF OBJECT_ID(N'[dbo].[Dioses]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Dioses](
        [IdDios] [int] IDENTITY(1,1) NOT NULL,
        [Nombre] [varchar](50) NOT NULL,
        [FotoDios] [varchar](150) NOT NULL,
        [Dialogo] [varchar](1000) NOT NULL,
     CONSTRAINT [PK_Dioses] PRIMARY KEY CLUSTERED 
    (
        [IdDios] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT 1 FROM [dbo].[Dioses])
BEGIN
    SET IDENTITY_INSERT [dbo].[Dioses] ON
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (1, N'Zeus', N'Zeus.png', N'¡Contemplá el poder del firmamento, pariente! Los demás te ofrecerán trucos sutiles, pero cuando el Tiempo te pisa los talones, nada supera a la fuerza de la tormenta. Elegí mi rayo: fulminaremos los obstáculos y ralentizaremos los mecanismos de tus enemigos. Dejá que mis hermanos duden, nosotros golpeamos primero.')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (2, N'Poseidon', N'Poseidon.png', N'¡Ja! No te dejes engañar por promesas elegantes, pequeña. Solo la fuerza de una marea implacable puede arrastrar la resistencia del enemigo. Con mi bendición, aturdiremos el compás del peligro y empujaremos las respuestas hacia vos como un naufragio en la costa. ¡Elegime y que rujan las olas!')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (3, N'Apolo', N'Apolo.png', N'¡Saludos, estrella de la noche! El camino hacia la cumbre es oscuro y lleno de desvíos engañosos, pero mi luz puede disipar cualquier sombra. Mi bendición iluminará el camino correcto y te dará más tiempo para memorizar los patrones del enemigo. Dejá que la claridad guíe tus pasos.')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (4, N'Hera', N'Hera.png', N'El linaje y el orden deben prevalecer ante el caos de Cronos. Las deidades menores te ofrecerán libertades efímeras, pero mi lazo soberano te otorga verdadero control. Con mi bendición, ataremos los elementos del acertijo para que un acierto debilite el resto de las trabas. Someté el nivel a tu voluntad.')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (5, N'Demeter', N'Demeter.png', N'El invierno no conoce la piedad, y tus enemigos tampoco deberían conocerla. Mientras los jóvenes del Olimpo derrochan palabras, mi escarcha congelará sus pretensiones. Si elegís mi favor, congelaremos los temporizadores del nivel, dándote la fría calma que necesitás para pensar sin presiones. Soportá la tormenta.')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (6, N'Hefesto', N'Hefesto.png', N'Los discursos bonitos no rompen cadenas, muchacha; el metal al rojo vivo sí. Mientras los demás te dan bendiciones intangibles, yo te ofrezco ingeniería pura y pesada. Mi favor destruirá una de las sub-fases más molestas de un solo golpe de mi martillo. Dejá la magia a un lado y elegí la fuerza del yunque.')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (7, N'Hestia', N'Hestia.png', N'En medio de la guerra y el caos, la llama del hogar es lo único que permanece puro. El fuego de los demás consume, pero el mío purifica y desgasta la resistencia de las trampas. Con mi bendición, consumiremos los errores del tablero, permitiéndote fallar sin sufrir el castigo completo del enemigo. Mantené la llama encendida.')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (8, N'Ares', N'Ares.png', N'¡La diplomacia ha terminado! Esta sala es un campo de batalla y la única salida es a través de la ruina de sus defensas. Olvidate de la paciencia o la lógica; mi bendición te otorgará una furia bélica que forzará la apertura de los candados reduciendo los requisitos del puzzle. Elegí la guerra y abrite paso.')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (9, N'Hermes', N'Hermes.png', N'¡Hola, hola! No hay tiempo que perder, ¡el reloj corre rapidísimo! Las demás deidades se toman demasiadas pausas para actuar, pero mi poder es inmediato. Si me elegís, te daré la agilidad mental necesaria para adelantarte a las trampas y reintentar tus movimientos antes de que el servidor registre un fallo. ¡Apuremos el paso!')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (10, N'Selene', N'Selene.png', N'La Luna observa todo desde lo alto, criatura de la noche, y conoce los secretos que los dioses del día ignoran. Mi luz argéntea no te dará fuerza, sino metamorfosis. Al invocarme, activaremos una habilidad oculta que alterará temporalmente las reglas de la sala a tu favor. Confiá en la plata de la noche.')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (11, N'Artemisa', N'Artemisa.png', N'No necesitás discursos largos ni templos ostentosos. Lo que necesitás es precisión implacable. Mientras los demás discuten en sus tronos, mi flecha va directo al punto crítico. Elegí mi favor y perforaremos las sub-fases más molestas, dándote un escape rápido. Movete rápido, elígeme.')
    INSERT [dbo].[Dioses] ([IdDios], [Nombre], [FotoDios], [Dialogo]) VALUES (12, N'Atenea', N'Atenea.png', N'La fuerza sin estrategia no es más que un despliegue vacío. Mis parientes te ofrecen caos, pero yo te ofrezco la verdad oculta tras el velo. Si aceptás mi escudo, descartaremos el error y traeremos claridad a tu mente para mirar a través de las trampas. Elegí la razón; la victoria se planifica.')
    SET IDENTITY_INSERT [dbo].[Dioses] OFF
END
GO
USE [master]
GO
ALTER DATABASE [Hades2SalaEscape] SET  READ_WRITE 
GO
