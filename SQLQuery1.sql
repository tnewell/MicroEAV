

DELETE [Value];
DELETE [Instance];
DBCC CHECKIDENT ([Instance], RESEED, 0);
DELETE [Subject];
DBCC CHECKIDENT ([Subject], RESEED, 0);
DELETE [Attribute];
DBCC CHECKIDENT ([Attribute], RESEED, 0);
DELETE [Container];
DBCC CHECKIDENT ([Container], RESEED, 0);
DELETE [Context];
DBCC CHECKIDENT ([Context], RESEED, 0);
DELETE [Entity];
DBCC CHECKIDENT ([Entity], RESEED, 0)


