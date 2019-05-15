DROP TABLE IF EXISTS MST_USER_INFO

CREATE TABLE MST_USER_INFO
(
		USER_NO			CHAR(6)			NOT NULL	PRIMARY KEY
	,	USER_ID			CHAR(20)		NOT NULL
	,	USER_NAME		VARCHAR(100)	NOT NULL
	,	PASSWORD		VARCHAR(100)	NOT NULL
	,	OVER_TIME		INT				NOT NULL
	,	PERCENT_TIME	INT				NOT NULL
)

INSERT INTO MST_USER_INFO
VALUES
(
		'000000'
	,	'Administrator'
	,	'管理者'
	,	'admin'
	,	30
	,	30
)

INSERT INTO MST_USER_INFO
VALUES
(
		'000001'
	,	'k.shiyo.no'
	,	'Shota Katsuyama'
	,	'XbL203tb'
	,	20
	,	20
)

INSERT INTO MST_USER_INFO
VALUES
(
		'999999'
	,	'a'
	,	'テスト用ユーザー'
	,	'a'
	,	20
	,	20
)
