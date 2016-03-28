--
-- Скрипт сгенерирован Devart dbForge Studio for SQL Server, Версия 5.1.178.0
-- Домашняя страница продукта: http://www.devart.com/ru/dbforge/sql/studio
-- Дата скрипта: 28.03.2016 20:35:53
-- Версия сервера: 12.00.0703
-- Версия клиента: 
--



--
-- Создать пользователя "appuser"
--
PRINT (N'Создать пользователя "appuser"')
GO
IF DATABASE_PRINCIPAL_ID(N'appuser') IS NULL
CREATE USER appuser
  WITHOUT LOGIN
GO

--
-- Создать роль "approle"
--
PRINT (N'Создать роль "approle"')
GO
IF DATABASE_PRINCIPAL_ID(N'approle') IS NULL
CREATE ROLE approle
GO

--
-- Добавить члены в роль "approle"
--
PRINT (N'Добавить члены в роль "approle"')
GO
EXEC sp_addrolemember N'approle', N'appuser'
GO

--
-- Создать таблицу "dbo.[User]"
--
PRINT (N'Создать таблицу "dbo.[User]"')
GO
IF OBJECT_ID(N'dbo.User', 'U') IS NULL
CREATE TABLE dbo.[User] (
  id uniqueidentifier NOT NULL,
  Login nvarchar(50) NOT NULL,
  Password uniqueidentifier NOT NULL,
  DateOfCreate datetime NOT NULL,
  CONSTRAINT PrimaryKey_user_id PRIMARY KEY CLUSTERED (id),
  CONSTRAINT IX_UB_User UNIQUE (Login)
)
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO

--
-- Создать процедуру "dbo.DeleteUser"
--
GO
PRINT (N'Создать процедуру "dbo.DeleteUser"')
GO
IF OBJECT_ID(N'dbo.DeleteUser', 'P') IS NULL
EXEC sp_executesql N'CREATE PROCEDURE dbo.DeleteUser
  @userid UNIQUEIDENTIFIER
AS 
  DELETE  
  FROM [User]
  WHERE id=@userid
  SELECT @@rowcount
'
GO

--
-- Предоставить разрешения на "dbo.DeleteUser"
--
PRINT (N'Предоставить разрешения на "dbo.DeleteUser"')
GO
IF OBJECT_ID(N'dbo.DeleteUser', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.DeleteUser TO approle
GO

--
-- Создать таблицу "dbo.TypeOfMyAnswer"
--
PRINT (N'Создать таблицу "dbo.TypeOfMyAnswer"')
GO
IF OBJECT_ID(N'dbo.TypeOfMyAnswer', 'U') IS NULL
CREATE TABLE dbo.TypeOfMyAnswer (
  id tinyint IDENTITY,
  Name nvarchar(20) NULL,
  Description nvarchar(50) NULL,
  CONSTRAINT PK_TypeOfMyAnswer PRIMARY KEY CLUSTERED (id),
  CONSTRAINT KEY_TypeOfMyAnswer_Name UNIQUE (Name)
)
GO

--
-- Создать процедуру "dbo.GetTypeOfMyAnswer"
--
GO
PRINT (N'Создать процедуру "dbo.GetTypeOfMyAnswer"')
GO
IF OBJECT_ID(N'dbo.GetTypeOfMyAnswer', 'P') IS NULL
EXEC sp_executesql N'CREATE PROCEDURE dbo.GetTypeOfMyAnswer
AS 
  SET NOCOUNT ON
  SELECT at.id as Id, at.Name, at.Description FROM TypeOfMyAnswer at
'
GO

--
-- Предоставить разрешения на "dbo.GetTypeOfMyAnswer"
--
PRINT (N'Предоставить разрешения на "dbo.GetTypeOfMyAnswer"')
GO
IF OBJECT_ID(N'dbo.GetTypeOfMyAnswer', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.GetTypeOfMyAnswer TO approle
GO

--
-- Создать таблицу "dbo.Tickets"
--
PRINT (N'Создать таблицу "dbo.Tickets"')
GO
IF OBJECT_ID(N'dbo.Tickets', 'U') IS NULL
CREATE TABLE dbo.Tickets (
  Id uniqueidentifier NOT NULL,
  User_id uniqueidentifier NOT NULL,
  T_IP_adress varchar(50) NOT NULL,
  T_DateOfCreate datetime NOT NULL,
  CONSTRAINT PrimaryKey_tickets_id PRIMARY KEY CLUSTERED (Id)
)
GO

--
-- Создать процедуру "dbo.CheckTicket"
--
GO
PRINT (N'Создать процедуру "dbo.CheckTicket"')
GO
IF OBJECT_ID(N'dbo.CheckTicket', 'P') IS NULL
EXEC sp_executesql N'/*Проверяем на корректность тикет*/
CREATE procedure dbo.CheckTicket 
  @userid uniqueidentifier,  
  @ticket uniqueidentifier, 
  @ip_adress VARCHAR(50),
  @current1 datetime
as
BEGIN
  SET NOCOUNT ON
    --удалили все тикеты, у которых срок годности прошел
    DELETE
      FROM Tickets
      WHERE [User_id] =@userid and DATEDIFF(HOUR, T_DateOfCreate, @current1)>1 -- тикет действует 1 час

  if (EXISTS(SELECT 1 
           from Tickets 
           where Id=@ticket and [User_id] =@userid and T_IP_adress =@ip_adress )) --если есть действующие тикеты
  begin
      UPDATE Tickets -- обновляем время действия тикета 
      SET T_DateOfCreate = GETUTCDATE()
      WHERE Id=@ticket
      SELECT 1
  END
ELSE  
     BEGIN
      SELECT 0
     end  
end
'
GO

--
-- Предоставить разрешения на "dbo.CheckTicket"
--
PRINT (N'Предоставить разрешения на "dbo.CheckTicket"')
GO
IF OBJECT_ID(N'dbo.CheckTicket', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.CheckTicket TO approle
GO

--
-- Создать таблицу "dbo.Questions"
--
PRINT (N'Создать таблицу "dbo.Questions"')
GO
IF OBJECT_ID(N'dbo.Questions', 'U') IS NULL
CREATE TABLE dbo.Questions (
  id uniqueidentifier NOT NULL,
  Text dbo.aqtext NOT NULL,
  Packadge_id uniqueidentifier NOT NULL,
  AnswerType_id tinyint NOT NULL,
  CONSTRAINT PK_Questions PRIMARY KEY CLUSTERED (id),
  CONSTRAINT KEY_Questions UNIQUE (Text, Packadge_id)
)
GO

--
-- Создать процедуру "dbo.GetQuestionsByPackage"
--
GO
PRINT (N'Создать процедуру "dbo.GetQuestionsByPackage"')
GO
IF OBJECT_ID(N'dbo.GetQuestionsByPackage', 'P') IS NULL
EXEC sp_executesql N'/*Получаем вопросы опросника*/
CREATE PROCEDURE dbo.GetQuestionsByPackage
  @package_id UNIQUEIDENTIFIER
AS 
  SET NOCOUNT ON
  SELECT q.id AS Id, q.[Text], q.AnswerType_id
  FROM Questions q
  WHERE q.Packadge_id=@package_id   
'
GO

--
-- Предоставить разрешения на "dbo.GetQuestionsByPackage"
--
PRINT (N'Предоставить разрешения на "dbo.GetQuestionsByPackage"')
GO
IF OBJECT_ID(N'dbo.GetQuestionsByPackage', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.GetQuestionsByPackage TO approle
GO

--
-- Создать таблицу "dbo.Package"
--
PRINT (N'Создать таблицу "dbo.Package"')
GO
IF OBJECT_ID(N'dbo.Package', 'U') IS NULL
CREATE TABLE dbo.Package (
  id uniqueidentifier NOT NULL,
  Name dbo.aqtext NOT NULL,
  User_id uniqueidentifier NOT NULL,
  CONSTRAINT PK_Package PRIMARY KEY CLUSTERED (id),
  CONSTRAINT KEY_Package UNIQUE (Name, User_id)
)
GO

--
-- Создать процедуру "dbo.SavePackage"
--
GO
PRINT (N'Создать процедуру "dbo.SavePackage"')
GO
IF OBJECT_ID(N'dbo.SavePackage', 'P') IS NULL
EXEC sp_executesql N'/*создание нового опросника*/
CREATE PROCEDURE dbo.SavePackage
  @id UNIQUEIDENTIFIER,
  @name aqtext,
  @userid UNIQUEIDENTIFIER
AS 
  IF (@name<>'''') -- проверяем аргументы на валидность 
  BEGIN
    IF (EXISTS (SELECT 1 FROM Package p WHERE p.id=@id AND p.User_id=@userid))--если существует опросник
      BEGIN --то его обновляем
        UPDATE Package 
        SET Name=@name
        WHERE id=@id 
      END
     ELSE
      BEGIN --иначе создаем
      INSERT Package (id, Name, User_id)
              VALUES (@id, @name, @userid);
      END   
  END
  SELECT @@rowcount

'
GO

--
-- Предоставить разрешения на "dbo.SavePackage"
--
PRINT (N'Предоставить разрешения на "dbo.SavePackage"')
GO
IF OBJECT_ID(N'dbo.SavePackage', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.SavePackage TO approle
GO

--
-- Создать процедуру "dbo.GetQuestionsByMyPackage"
--
GO
PRINT (N'Создать процедуру "dbo.GetQuestionsByMyPackage"')
GO
IF OBJECT_ID(N'dbo.GetQuestionsByMyPackage', 'P') IS NULL
EXEC sp_executesql N'/*Получаем вопросы опросника*/
CREATE PROCEDURE dbo.GetQuestionsByMyPackage
  @package_id UNIQUEIDENTIFIER,
  @user_id UNIQUEIDENTIFIER
AS 
  SET NOCOUNT ON
  SELECT q.id as Id, q.[Text], q.AnswerType_id
  FROM Questions q
  JOIN Package p ON q.Packadge_id = p.id
  WHERE q.Packadge_id=@package_id AND p.[User_id]=@user_id
'
GO

--
-- Предоставить разрешения на "dbo.GetQuestionsByMyPackage"
--
PRINT (N'Предоставить разрешения на "dbo.GetQuestionsByMyPackage"')
GO
IF OBJECT_ID(N'dbo.GetQuestionsByMyPackage', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.GetQuestionsByMyPackage TO approle
GO

--
-- Создать процедуру "dbo.GetPackage"
--
GO
PRINT (N'Создать процедуру "dbo.GetPackage"')
GO
IF OBJECT_ID(N'dbo.GetPackage', 'P') IS NULL
EXEC sp_executesql N'/*получение доступных опросников*/
CREATE PROCEDURE dbo.GetPackage
  @userid UNIQUEIDENTIFIER
AS 
  SET NOCOUNT ON
  SELECT p.id as Id, p.Name 
  FROM Package p
  WHERE p.User_id<>@userid
'
GO

--
-- Предоставить разрешения на "dbo.GetPackage"
--
PRINT (N'Предоставить разрешения на "dbo.GetPackage"')
GO
IF OBJECT_ID(N'dbo.GetPackage', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.GetPackage TO approle
GO

--
-- Создать процедуру "dbo.GetMyPackage"
--
GO
PRINT (N'Создать процедуру "dbo.GetMyPackage"')
GO
IF OBJECT_ID(N'dbo.GetMyPackage', 'P') IS NULL
EXEC sp_executesql N'/*получение моих опросников*/
CREATE PROCEDURE dbo.GetMyPackage
  @userid UNIQUEIDENTIFIER
AS 
  SET NOCOUNT ON
  SELECT p.id, p.Name 
  FROM Package p
  WHERE p.User_id=@userid
'
GO

--
-- Предоставить разрешения на "dbo.GetMyPackage"
--
PRINT (N'Предоставить разрешения на "dbo.GetMyPackage"')
GO
IF OBJECT_ID(N'dbo.GetMyPackage', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.GetMyPackage TO approle
GO

--
-- Создать процедуру "dbo.DeleteQuestion"
--
GO
PRINT (N'Создать процедуру "dbo.DeleteQuestion"')
GO
IF OBJECT_ID(N'dbo.DeleteQuestion', 'P') IS NULL
EXEC sp_executesql N'/*удаляем вопрос*/
CREATE PROCEDURE dbo.DeleteQuestion
  @id UNIQUEIDENTIFIER,
  @user_id UNIQUEIDENTIFIER
AS 
  BEGIN
  	DELETE
    FROM Questions
    WHERE id=@id AND Packadge_id = (SELECT top 1 p.id 
                                    FROM Package p
                                    JOIN Questions q ON p.id = q.Packadge_id  
                                    WHERE q.id=@id AND p.[User_id] = @user_id)
    SELECT @@rowcount
  END
'
GO

--
-- Предоставить разрешения на "dbo.DeleteQuestion"
--
PRINT (N'Предоставить разрешения на "dbo.DeleteQuestion"')
GO
IF OBJECT_ID(N'dbo.DeleteQuestion', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.DeleteQuestion TO approle
GO

--
-- Создать процедуру "dbo.DeletePackage"
--
GO
PRINT (N'Создать процедуру "dbo.DeletePackage"')
GO
IF OBJECT_ID(N'dbo.DeletePackage', 'P') IS NULL
EXEC sp_executesql N'/*удаляем опросник*/
CREATE PROCEDURE dbo.DeletePackage
  @userid UNIQUEIDENTIFIER,
  @id UNIQUEIDENTIFIER
AS  
    DELETE
    FROM Package
    WHERE id=@id AND User_id=@userid
  SELECT @@rowcount
'
GO

--
-- Предоставить разрешения на "dbo.DeletePackage"
--
PRINT (N'Предоставить разрешения на "dbo.DeletePackage"')
GO
IF OBJECT_ID(N'dbo.DeletePackage', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.DeletePackage TO approle
GO

--
-- Создать представление "dbo.QuestionsPackage"
--
GO
PRINT (N'Создать представление "dbo.QuestionsPackage"')
GO
IF OBJECT_ID(N'dbo.QuestionsPackage', 'V') IS NULL
EXEC sp_executesql N'CREATE VIEW dbo.QuestionsPackage 
AS
 SELECT  q.id as QuestionsId
        ,p.id AS PackageId      
        ,p.User_id AS UserId
  FROM Questions q
  JOIN Package p ON q.Packadge_id = p.id
'
GO

--
-- Создать процедуру "dbo.SaveQuestions"
--
GO
PRINT (N'Создать процедуру "dbo.SaveQuestions"')
GO
IF OBJECT_ID(N'dbo.SaveQuestions', 'P') IS NULL
EXEC sp_executesql N'/*создание нового вопроса*/
CREATE PROCEDURE dbo.SaveQuestions
  @id UNIQUEIDENTIFIER,
  @userid UNIQUEIDENTIFIER,
  @text aqtext,
  @package_id UNIQUEIDENTIFIER,
  @answertype_id TINYINT
AS 
  BEGIN
  IF (@text<>'''') --проверяем на валидность аругмент
  BEGIN
    IF (EXISTS(SELECT 1 
                FROM QuestionsPackage               
                WHERE QuestionsId=@id AND UserId=@userid AND PackageId=@package_id
      )) --если существует вопрос
      BEGIN --то обновляем поля
        UPDATE Questions
          SET
              [Text] = @text             
              ,AnswerType_id = @answertype_id
        WHERE id = @id;
      END
    ELSE
      BEGIN --иначе создаем новый вопрос
      	IF (EXISTS(SELECT 1 FROM Package p WHERE p.id=@package_id AND p.User_id=@userid))--но проверив существование опросника
          BEGIN
            INSERT Questions (id, [Text], Packadge_id, AnswerType_id)
                      VALUES (@id, @text, @package_id, @answertype_id); 
          END
      END   
  END
  SELECT @@rowcount
  END
'
GO

--
-- Предоставить разрешения на "dbo.SaveQuestions"
--
PRINT (N'Предоставить разрешения на "dbo.SaveQuestions"')
GO
IF OBJECT_ID(N'dbo.SaveQuestions', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.SaveQuestions TO approle
GO

--
-- Создать таблицу "dbo.MyAnswers"
--
PRINT (N'Создать таблицу "dbo.MyAnswers"')
GO
IF OBJECT_ID(N'dbo.MyAnswers', 'U') IS NULL
CREATE TABLE dbo.MyAnswers (
  id uniqueidentifier NOT NULL,
  User_id uniqueidentifier NOT NULL,
  Questions_id uniqueidentifier NOT NULL,
  TypeOfMyAnswer_id tinyint NOT NULL DEFAULT (1),
  Answer_id uniqueidentifier NULL,
  TextOfAnswer dbo.aqtext NULL,
  CONSTRAINT PK_MyAnswers PRIMARY KEY CLUSTERED (id)
)
GO

--
-- Создать процедуру "dbo.SaveMyAnswer"
--
GO
PRINT (N'Создать процедуру "dbo.SaveMyAnswer"')
GO
IF OBJECT_ID(N'dbo.SaveMyAnswer', 'P') IS NULL
EXEC sp_executesql N'/*сохраняем мои ответы*/
CREATE PROCEDURE dbo.SaveMyAnswer
  @id UNIQUEIDENTIFIER,
  @user_id UNIQUEIDENTIFIER,
  @question_id UNIQUEIDENTIFIER,
  @typeofmyanswer_id TINYINT,
  @answer_id UNIQUEIDENTIFIER NULL,
  @text aqtext NULL
AS 
  IF (@answer_id IS NOT NULL OR (@text is NOT NULL AND @text <> '''') AND @typeofmyanswer_id<3) --проверяем аргументы
  BEGIN
  	IF (EXISTS (SELECT 1 FROM MyAnswers ma WHERE ma.id=@id AND ma.[User_id] = @user_id AND ma.TypeOfMyAnswer_id = 1)) --если сущетвует ответ 
       BEGIN 
        UPDATE MyAnswers --обновляем
        SET
          TypeOfMyAnswer_id = @typeofmyanswer_id
          ,Answer_id = @answer_id
          ,TextOfAnswer = @text
        WHERE id = @id;
       END
    ELSE
      BEGIN --иначе создаем
      	INSERT MyAnswers (id, User_id, Questions_id, TypeOfMyAnswer_id, Answer_id, TextOfAnswer)
                 VALUES (@id, @user_id, @question_id, DEFAULT, @answer_id, @text);
      END   
  END
  SELECT @@rowcount
'
GO

--
-- Предоставить разрешения на "dbo.SaveMyAnswer"
--
PRINT (N'Предоставить разрешения на "dbo.SaveMyAnswer"')
GO
IF OBJECT_ID(N'dbo.SaveMyAnswer', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.SaveMyAnswer TO approle
GO

--
-- Создать процедуру "dbo.CheckAnswer"
--
GO
PRINT (N'Создать процедуру "dbo.CheckAnswer"')
GO
IF OBJECT_ID(N'dbo.CheckAnswer', 'P') IS NULL
EXEC sp_executesql N'/*проверяем ответ на вопрос*/
CREATE PROCEDURE dbo.CheckAnswer
  @userid UNIQUEIDENTIFIER,
  @answerid UNIQUEIDENTIFIER,
  @typeofanswer TINYINT
AS 
  IF (@typeofanswer>2) -- что бы созадтель вопроса не смогу ввести другой тип, кроме как "верен" или "неверен"
  BEGIN
    IF (EXISTS (SELECT 1
                FROM MyAnswers ma
                JOIN QuestionsPackage qp ON qp.QuestionsId = ma.Questions_id
                WHERE qp.UserId=@userid AND ma.User_id<>@userid and ma.id=@answerid
        )) --проверяем что есть такой ответ и он не создателя вопроса
      BEGIN
  	    UPDATE  MyAnswers
        SET 
          TypeOfMyAnswer_id = @typeofanswer   
        WHERE id = @answerid;
      END
  END
'
GO

--
-- Предоставить разрешения на "dbo.CheckAnswer"
--
PRINT (N'Предоставить разрешения на "dbo.CheckAnswer"')
GO
IF OBJECT_ID(N'dbo.CheckAnswer', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.CheckAnswer TO approle
GO

--
-- Создать таблицу "dbo.AnswerType"
--
PRINT (N'Создать таблицу "dbo.AnswerType"')
GO
IF OBJECT_ID(N'dbo.AnswerType', 'U') IS NULL
CREATE TABLE dbo.AnswerType (
  id tinyint IDENTITY,
  Name nvarchar(20) NOT NULL,
  Description nvarchar(50) NULL,
  CONSTRAINT PK_AnswerType PRIMARY KEY CLUSTERED (id),
  CONSTRAINT KEY_AnswerType_Name UNIQUE (Name)
)
GO

--
-- Создать процедуру "dbo.GetTypeOfAnswer"
--
GO
PRINT (N'Создать процедуру "dbo.GetTypeOfAnswer"')
GO
IF OBJECT_ID(N'dbo.GetTypeOfAnswer', 'P') IS NULL
EXEC sp_executesql N'CREATE PROCEDURE dbo.GetTypeOfAnswer
AS 
  SET NOCOUNT ON
  SELECT at.id as Id, at.Name, at.Description FROM AnswerType at
'
GO

--
-- Предоставить разрешения на "dbo.GetTypeOfAnswer"
--
PRINT (N'Предоставить разрешения на "dbo.GetTypeOfAnswer"')
GO
IF OBJECT_ID(N'dbo.GetTypeOfAnswer', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.GetTypeOfAnswer TO approle
GO

--
-- Создать таблицу "dbo.Answers"
--
PRINT (N'Создать таблицу "dbo.Answers"')
GO
IF OBJECT_ID(N'dbo.Answers', 'U') IS NULL
CREATE TABLE dbo.Answers (
  id uniqueidentifier NOT NULL,
  Text dbo.aqtext NOT NULL,
  Questions_id uniqueidentifier NOT NULL,
  CONSTRAINT PK_Answers PRIMARY KEY CLUSTERED (id),
  CONSTRAINT KEY_Answers UNIQUE (Text, Questions_id)
)
GO

--
-- Создать процедуру "dbo.SaveAnswer"
--
GO
PRINT (N'Создать процедуру "dbo.SaveAnswer"')
GO
IF OBJECT_ID(N'dbo.SaveAnswer', 'P') IS NULL
EXEC sp_executesql N'/*сохраняем ответ на вопрос*/
CREATE PROCEDURE dbo.SaveAnswer
  @userid UNIQUEIDENTIFIER,
  @id UNIQUEIDENTIFIER,
  @text aqtext,
  @question_id UNIQUEIDENTIFIER
AS 
  IF (@text <>'''') --проверяем на валидность аргумент
  BEGIN
  	IF (EXISTS(SELECT 1
             FROM Answers a
             JOIN QuestionsPackage qp ON a.Questions_id = qp.QuestionsId             
             WHERE a.id=@id AND a.Questions_id=@question_id AND qp.UserId=@userid)) --если существует ответ на вопрос
    BEGIN
    	UPDATE Answers --то мы его обновляем
      SET 
         [Text] = @text 
      WHERE id = @id;
    END
    ELSE
    BEGIN --иначе
      IF (EXISTS(SELECT 1 
                 FROM Questions q
                 JOIN Package p ON p.id = q.Packadge_id
                 WHERE p.User_id=@userid AND q.id=@question_id AND q.AnswerType_id<>3)) -- проверяем что существует такой вопрос и на него ответ нужно выбрать
        BEGIN
        	INSERT  Answers (id, [Text], Questions_id) --создаем новый ответ
                   VALUES (@id, @text, @question_id);
        END
    END   
  END 
  SELECT @@rowcount
'
GO

--
-- Предоставить разрешения на "dbo.SaveAnswer"
--
PRINT (N'Предоставить разрешения на "dbo.SaveAnswer"')
GO
IF OBJECT_ID(N'dbo.SaveAnswer', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.SaveAnswer TO approle
GO

--
-- Создать процедуру "dbo.GetAnswerForMyQuestion"
--
GO
PRINT (N'Создать процедуру "dbo.GetAnswerForMyQuestion"')
GO
IF OBJECT_ID(N'dbo.GetAnswerForMyQuestion', 'P') IS NULL
EXEC sp_executesql N'CREATE PROCEDURE dbo.GetAnswerForMyQuestion
  @userid UNIQUEIDENTIFIER,
  @question UNIQUEIDENTIFIER
AS 
  SET NOCOUNT ON
  SELECT a.id, a.[Text] 
  FROM Answers a
  JOIN QuestionsPackage qp ON a.Questions_id=qp.QuestionsId AND qp.UserId =@userid
'
GO

--
-- Предоставить разрешения на "dbo.GetAnswerForMyQuestion"
--
PRINT (N'Предоставить разрешения на "dbo.GetAnswerForMyQuestion"')
GO
IF OBJECT_ID(N'dbo.GetAnswerForMyQuestion', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.GetAnswerForMyQuestion TO approle
GO

--
-- Создать процедуру "dbo.GetAnswer"
--
GO
PRINT (N'Создать процедуру "dbo.GetAnswer"')
GO
IF OBJECT_ID(N'dbo.GetAnswer', 'P') IS NULL
EXEC sp_executesql N'CREATE PROCEDURE dbo.GetAnswer
  @question UNIQUEIDENTIFIER
AS 
  SET NOCOUNT ON
  SELECT a.id, a.[Text]
  FROM Answers a
  WHERE a.Questions_id=@question
'
GO

--
-- Предоставить разрешения на "dbo.GetAnswer"
--
PRINT (N'Предоставить разрешения на "dbo.GetAnswer"')
GO
IF OBJECT_ID(N'dbo.GetAnswer', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.GetAnswer TO approle
GO

--
-- Создать процедуру "dbo.DeleteAnswer"
--
GO
PRINT (N'Создать процедуру "dbo.DeleteAnswer"')
GO
IF OBJECT_ID(N'dbo.DeleteAnswer', 'P') IS NULL
EXEC sp_executesql N'CREATE PROCEDURE dbo.DeleteAnswer
  @answerid UNIQUEIDENTIFIER,
  @userid UNIQUEIDENTIFIER 
AS 
 DELETE
  FROM Answers
  WHERE id=@answerid AND Questions_id = (SELECT TOP 1 qp.QuestionsId
                                         from Answers a 
                                         JOIN QuestionsPackage qp ON qp.UserId=@userid
                                         WHERE a.id=@answerid )
  SELECT @@rowcount
'
GO

--
-- Предоставить разрешения на "dbo.DeleteAnswer"
--
PRINT (N'Предоставить разрешения на "dbo.DeleteAnswer"')
GO
IF OBJECT_ID(N'dbo.DeleteAnswer', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.DeleteAnswer TO approle
GO

--
-- Создать функцию "dbo.CreateHashForPassword"
--
GO
PRINT (N'Создать функцию "dbo.CreateHashForPassword"')
GO
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.CreateHashForPassword') AND type IN ('IF', 'FN', 'TF'))
EXEC sp_executesql N'/*функция по созданию хэша*/
CREATE FUNCTION dbo.CreateHashForPassword (@password NVARCHAR(50), @regdate DATETIME)
  RETURNS UNIQUEIDENTIFIER
  AS
  BEGIN
  DECLARE @str VARCHAR(75) =CONCAT(CAST(@regdate AS NVARCHAR(25)),@password)--получаем строчку, которую будем хэшировать
  RETURN HASHBYTES(''MD5'', @str)
  end
'
GO

--
-- Создать процедуру "dbo.RegUser"
--
GO
PRINT (N'Создать процедуру "dbo.RegUser"')
GO
IF OBJECT_ID(N'dbo.RegUser', 'P') IS NULL
EXEC sp_executesql N'/*создание нового пользователя*/
CREATE procedure dbo.RegUser  
  @id UNIQUEIDENTIFIER,
  @login nvarchar(50), 
  @password NVARCHAR(MAX) 
AS
BEGIN
IF (@login IS NOT NULL AND @login<>'''' AND @password IS NOT NULL AND @password<>'''')
  BEGIN
    DECLARE @date DATETIME = GETDATE(); -- на основе даты будем делать соль к хэшу
    INSERT [User] (id, [Login], [Password], DateOfCreate)
    VALUES (@id, @login, dbo.CreateHashForPassword(@password, @date), @date);   
  END
   SELECT @@rowcount
END

'
GO

--
-- Предоставить разрешения на "dbo.RegUser"
--
PRINT (N'Предоставить разрешения на "dbo.RegUser"')
GO
IF OBJECT_ID(N'dbo.RegUser', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.RegUser TO approle
GO

--
-- Создать процедуру "dbo.LogOnUser"
--
GO
PRINT (N'Создать процедуру "dbo.LogOnUser"')
GO
IF OBJECT_ID(N'dbo.LogOnUser', 'P') IS NULL
EXEC sp_executesql N'/*вход пользователя*/
CREATE procedure dbo.LogOnUser 
@login nvarchar(50),
@password NVARCHAR(MAX),
@ip_adress varchar(50)
as
SET NOCOUNT ON
BEGIN 
declare @tem UNIQUEIDENTIFIER --запишем id пользователя сюда

SELECT TOP 1 @tem =  u.id --получили id пользователя
from [User] u
where u.[Login]=@login and u.[Password]=dbo.CreateHashForPassword(@password,u.DateOfCreate)

  IF (@tem IS NOT NULL)
    BEGIN
      DELETE --удаляем тикеты которые весят на данном ip
      FROM Tickets
      WHERE [User_id]=@tem AND T_IP_adress=@ip_adress

      DECLARE @str VARCHAR(125) =CONCAT(CAST(SYSDATETIME() AS NVARCHAR(25)), CAST(@tem AS VARCHAR(50)),@login,@ip_adress)--получаем строчку, которую будем хэшировать
      DECLARE @ticket UNIQUEIDENTIFIER = HASHBYTES(''MD5'',@str)--создаем новый тикет
      INSERT Tickets (Id, [User_id], T_IP_adress, T_DateOfCreate) --сохраняем
              VALUES (@ticket, @tem, @ip_adress, GETUTCDATE());

      SELECT @tem AS Id    --1
        , @login AS [Login]--2
        , @ticket AS [Password] --3
    END
END
'
GO

--
-- Предоставить разрешения на "dbo.LogOnUser"
--
PRINT (N'Предоставить разрешения на "dbo.LogOnUser"')
GO
IF OBJECT_ID(N'dbo.LogOnUser', 'P') IS NOT NULL

  AND DATABASE_PRINCIPAL_ID(N'approle') IS NOT NULL
GRANT EXECUTE ON dbo.LogOnUser TO approle
GO
-- 
-- Вывод данных для таблицы Answers
--
-- Таблица AaQ_test.dbo.Answers не содержит данных
-- 
-- Вывод данных для таблицы AnswerType
--
SET IDENTITY_INSERT dbo.AnswerType ON
GO
INSERT dbo.AnswerType(id, Name, Description) VALUES (1, N'Мультивыбор', N'Позволяет выбирать несколько ответов в вопросе')
INSERT dbo.AnswerType(id, Name, Description) VALUES (2, N'Один выбор', N'Только один ответ в вопросе')
INSERT dbo.AnswerType(id, Name, Description) VALUES (3, N'Свободный ответ', N'Ответ нужно написать')
GO
SET IDENTITY_INSERT dbo.AnswerType OFF
GO
-- 
-- Вывод данных для таблицы MyAnswers
--
-- Таблица AaQ_test.dbo.MyAnswers не содержит данных
-- 
-- Вывод данных для таблицы Package
--
-- Таблица AaQ_test.dbo.Package не содержит данных
-- 
-- Вывод данных для таблицы Questions
--
-- Таблица AaQ_test.dbo.Questions не содержит данных
-- 
-- Вывод данных для таблицы Tickets
--
INSERT dbo.Tickets(Id, User_id, T_IP_adress, T_DateOfCreate) VALUES ('cb781a60-0317-b188-6a01-aa9b2357cfcb', '5cdbdf50-5f64-4f22-ad9e-7234bf4058dd', N'::1', '2016-03-27 16:16:19.397')
GO
-- 
-- Вывод данных для таблицы TypeOfMyAnswer
--
SET IDENTITY_INSERT dbo.TypeOfMyAnswer ON
GO
INSERT dbo.TypeOfMyAnswer(id, Name, Description) VALUES (1, N'Ответ введен', N'Пользователь ввел ответ')
INSERT dbo.TypeOfMyAnswer(id, Name, Description) VALUES (2, N'Ответ подвержден', N'Пользователь подвердил свой ответ')
INSERT dbo.TypeOfMyAnswer(id, Name, Description) VALUES (3, N'Ответ верный', N'Ответ проверен создателем вопроса и он верный')
INSERT dbo.TypeOfMyAnswer(id, Name, Description) VALUES (4, N'Ответ неверный', N'Ответ проверен создателем вопроса и он неверный')
GO
SET IDENTITY_INSERT dbo.TypeOfMyAnswer OFF
GO
-- 
-- Вывод данных для таблицы [User]
--
INSERT dbo.[User](id, Login, Password, DateOfCreate) VALUES ('5cdbdf50-5f64-4f22-ad9e-7234bf4058dd', N's18', '7eff67af-7e6d-6bcc-e18d-9cefbe51ad6c', '2016-03-27 15:35:52.240')
GO

--
-- Создать внешний ключ "FK_Tickets_User_id" для объекта типа таблица "dbo.Tickets"
--
PRINT (N'Создать внешний ключ "FK_Tickets_User_id" для объекта типа таблица "dbo.Tickets"')
GO
IF OBJECT_ID('dbo.FK_Tickets_User_id', 'F') IS NULL
  AND OBJECT_ID('dbo.Tickets', 'U') IS NOT NULL
  AND OBJECT_ID('dbo.User', 'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.Tickets', N'User_id') IS NOT NULL
  AND COL_LENGTH(N'dbo.User', N'id') IS NOT NULL
ALTER TABLE dbo.Tickets
  ADD CONSTRAINT FK_Tickets_User_id FOREIGN KEY (User_id) REFERENCES dbo.[User] (id) ON DELETE CASCADE ON UPDATE CASCADE
GO

--
-- Создать внешний ключ "FK_Package_User_id" для объекта типа таблица "dbo.Package"
--
PRINT (N'Создать внешний ключ "FK_Package_User_id" для объекта типа таблица "dbo.Package"')
GO
IF OBJECT_ID('dbo.FK_Package_User_id', 'F') IS NULL
  AND OBJECT_ID('dbo.Package', 'U') IS NOT NULL
  AND OBJECT_ID('dbo.User', 'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.Package', N'User_id') IS NOT NULL
  AND COL_LENGTH(N'dbo.User', N'id') IS NOT NULL
ALTER TABLE dbo.Package
  ADD CONSTRAINT FK_Package_User_id FOREIGN KEY (User_id) REFERENCES dbo.[User] (id) ON DELETE CASCADE ON UPDATE CASCADE
GO

--
-- Создать внешний ключ "FK_Questions_AnswerType_id" для объекта типа таблица "dbo.Questions"
--
PRINT (N'Создать внешний ключ "FK_Questions_AnswerType_id" для объекта типа таблица "dbo.Questions"')
GO
IF OBJECT_ID('dbo.FK_Questions_AnswerType_id', 'F') IS NULL
  AND OBJECT_ID('dbo.Questions', 'U') IS NOT NULL
  AND OBJECT_ID('dbo.AnswerType', 'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.Questions', N'AnswerType_id') IS NOT NULL
  AND COL_LENGTH(N'dbo.AnswerType', N'id') IS NOT NULL
ALTER TABLE dbo.Questions
  ADD CONSTRAINT FK_Questions_AnswerType_id FOREIGN KEY (AnswerType_id) REFERENCES dbo.AnswerType (id)
GO

--
-- Создать внешний ключ "FK_Questions_Package_id" для объекта типа таблица "dbo.Questions"
--
PRINT (N'Создать внешний ключ "FK_Questions_Package_id" для объекта типа таблица "dbo.Questions"')
GO
IF OBJECT_ID('dbo.FK_Questions_Package_id', 'F') IS NULL
  AND OBJECT_ID('dbo.Questions', 'U') IS NOT NULL
  AND OBJECT_ID('dbo.Package', 'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.Questions', N'Packadge_id') IS NOT NULL
  AND COL_LENGTH(N'dbo.Package', N'id') IS NOT NULL
ALTER TABLE dbo.Questions
  ADD CONSTRAINT FK_Questions_Package_id FOREIGN KEY (Packadge_id) REFERENCES dbo.Package (id) ON DELETE CASCADE ON UPDATE CASCADE
GO

--
-- Создать внешний ключ "FK_Answers_Questions_id" для объекта типа таблица "dbo.Answers"
--
PRINT (N'Создать внешний ключ "FK_Answers_Questions_id" для объекта типа таблица "dbo.Answers"')
GO
IF OBJECT_ID('dbo.FK_Answers_Questions_id', 'F') IS NULL
  AND OBJECT_ID('dbo.Answers', 'U') IS NOT NULL
  AND OBJECT_ID('dbo.Questions', 'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.Answers', N'Questions_id') IS NOT NULL
  AND COL_LENGTH(N'dbo.Questions', N'id') IS NOT NULL
ALTER TABLE dbo.Answers
  ADD CONSTRAINT FK_Answers_Questions_id FOREIGN KEY (Questions_id) REFERENCES dbo.Questions (id) ON DELETE CASCADE ON UPDATE CASCADE
GO

--
-- Создать внешний ключ "FK_MyAnswers_Answers_id" для объекта типа таблица "dbo.MyAnswers"
--
PRINT (N'Создать внешний ключ "FK_MyAnswers_Answers_id" для объекта типа таблица "dbo.MyAnswers"')
GO
IF OBJECT_ID('dbo.FK_MyAnswers_Answers_id', 'F') IS NULL
  AND OBJECT_ID('dbo.MyAnswers', 'U') IS NOT NULL
  AND OBJECT_ID('dbo.Answers', 'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.MyAnswers', N'Answer_id') IS NOT NULL
  AND COL_LENGTH(N'dbo.Answers', N'id') IS NOT NULL
ALTER TABLE dbo.MyAnswers
  ADD CONSTRAINT FK_MyAnswers_Answers_id FOREIGN KEY (Answer_id) REFERENCES dbo.Answers (id)
GO

--
-- Создать внешний ключ "FK_MyAnswers_Questions_id" для объекта типа таблица "dbo.MyAnswers"
--
PRINT (N'Создать внешний ключ "FK_MyAnswers_Questions_id" для объекта типа таблица "dbo.MyAnswers"')
GO
IF OBJECT_ID('dbo.FK_MyAnswers_Questions_id', 'F') IS NULL
  AND OBJECT_ID('dbo.MyAnswers', 'U') IS NOT NULL
  AND OBJECT_ID('dbo.Questions', 'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.MyAnswers', N'Questions_id') IS NOT NULL
  AND COL_LENGTH(N'dbo.Questions', N'id') IS NOT NULL
ALTER TABLE dbo.MyAnswers
  ADD CONSTRAINT FK_MyAnswers_Questions_id FOREIGN KEY (Questions_id) REFERENCES dbo.Questions (id)
GO

--
-- Создать внешний ключ "FK_MyAnswers_TypeOfMyAnswer_id" для объекта типа таблица "dbo.MyAnswers"
--
PRINT (N'Создать внешний ключ "FK_MyAnswers_TypeOfMyAnswer_id" для объекта типа таблица "dbo.MyAnswers"')
GO
IF OBJECT_ID('dbo.FK_MyAnswers_TypeOfMyAnswer_id', 'F') IS NULL
  AND OBJECT_ID('dbo.MyAnswers', 'U') IS NOT NULL
  AND OBJECT_ID('dbo.TypeOfMyAnswer', 'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.MyAnswers', N'TypeOfMyAnswer_id') IS NOT NULL
  AND COL_LENGTH(N'dbo.TypeOfMyAnswer', N'id') IS NOT NULL
ALTER TABLE dbo.MyAnswers
  ADD CONSTRAINT FK_MyAnswers_TypeOfMyAnswer_id FOREIGN KEY (TypeOfMyAnswer_id) REFERENCES dbo.TypeOfMyAnswer (id)
GO

--
-- Создать внешний ключ "FK_MyAnswers_User_id" для объекта типа таблица "dbo.MyAnswers"
--
PRINT (N'Создать внешний ключ "FK_MyAnswers_User_id" для объекта типа таблица "dbo.MyAnswers"')
GO
IF OBJECT_ID('dbo.FK_MyAnswers_User_id', 'F') IS NULL
  AND OBJECT_ID('dbo.MyAnswers', 'U') IS NOT NULL
  AND OBJECT_ID('dbo.User', 'U') IS NOT NULL
  AND COL_LENGTH(N'dbo.MyAnswers', N'User_id') IS NOT NULL
  AND COL_LENGTH(N'dbo.User', N'id') IS NOT NULL
ALTER TABLE dbo.MyAnswers
  ADD CONSTRAINT FK_MyAnswers_User_id FOREIGN KEY (User_id) REFERENCES dbo.[User] (id) ON DELETE CASCADE ON UPDATE CASCADE
GO
SET NOEXEC OFF
GO