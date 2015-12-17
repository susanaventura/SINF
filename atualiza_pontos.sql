update ArtigoMoeda set PVP6=0;
update ArtigoMoeda set PVP6=CEILING(PVP1*10) where Artigo in (select top 10 percent Artigo from ArtigoMoeda order by NEWID());


select * from ArtigoMoeda;