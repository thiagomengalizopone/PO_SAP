CREATE VIEW ZPN_VW_POSITE AS 


SELECT 
	POList.po_id,
	POList.shipmentnum,
	CASE 
        WHEN CHARINDEX('<!>', manufactureSiteInfo, CHARINDEX('<!>', manufactureSiteInfo) + 1) > 0
             AND CHARINDEX('_', manufactureSiteInfo + ' ') > CHARINDEX('<!>', manufactureSiteInfo, CHARINDEX('<!>', manufactureSiteInfo) + 1) 
        THEN 
            SUBSTRING(
                manufactureSiteInfo, 
                CHARINDEX('<!>', manufactureSiteInfo, CHARINDEX('<!>', manufactureSiteInfo) + 1) + 3, 
                CHARINDEX('_', manufactureSiteInfo + ' ') - CHARINDEX('<!>', manufactureSiteInfo, CHARINDEX('<!>', manufactureSiteInfo) + 1) - 3
            )
        ELSE 
            NULL
    END AS ExtractedValue
FROM 
    [192.168.8.241,15050].Zopone.dbo.POList
	