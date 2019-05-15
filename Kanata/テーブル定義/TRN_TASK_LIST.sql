DROP TABLE IF EXISTS TRN_TASK_LIST

CREATE TABLE TRN_TASK_LIST
(
		TASK_NO					CHAR(7)			NOT NULL
	,	USER_NO					CHAR(6)			NOT NULL
	,	CREATE_YMD				DATE			NOT NULL
	,	UPDATE_YMD				DATE			NULL
	,	TODO_YMD				DATE			NOT NULL
	,	FINISHED_YMD			DATE			NULL
	,	TASK_STATUS_CODE		NCHAR(2)		NOT NULL
	,	TASK_KIND_CODE			NCHAR(2)		NOT NULL
	,	TASK_GROUP_CODE			NCHAR(2)		NOT NULL
	,	TASK_NAME				VARCHAR(100)	NOT NULL
	,	PLAN_TIME				TIME			NOT NULL
	,	RESULT_TIME				TIME			NOT NULL
	,	MEMO					VARCHAR(400)	NULL
	PRIMARY KEY(TASK_NO, USER_NO)
)

--	INSERT INTO TRN_TASK_LIST
--	VALUES
--	(
--			'0000001'
--		,	'000000'
--		,	'20190309'
--		,	NULL
--		,	'20190309'
--		,	'01'
--		,	'01'
--		,	'01'
--		,	'テスト用タスク１'
--		,	'5:00'
--		,	'10:00'
--		,	'１５時にＡさんと●●会議室で実施予定。'
--	)
--	
--	INSERT INTO TRN_TASK_LIST
--	VALUES
--	(
--			'0000002'
--		,	'000000'
--		,	'20190310'
--		,	NULL
--		,	'20190310'
--		,	'01'
--		,	'01'
--		,	'05'
--		,	'テスト用タスク２'
--		,	'1:30'
--		,	'2:00'
--		,	'●●が必要。'
--	)
--	
--	INSERT INTO TRN_TASK_LIST
--	VALUES
--	(
--			'0000003'
--		,	'000000'
--		,	'20190305'
--		,	NULL
--		,	'20190305'
--		,	'20'
--		,	'20'
--		,	'15'
--		,	'テスト用タスク３'
--		,	'2:30'
--		,	'2:00'
--		,	''
--	)