CREATE VIEW ZPN_VW_POSITE AS 


SELECT 
	POList.po_id,
	POList.poLineNum,

	
	CASE 
        WHEN CHARINDEX('<!>', replace(manufactureSiteInfo, 'PIN_',''), CHARINDEX('<!>', replace(manufactureSiteInfo, 'PIN_','')) + 1) > 0
             AND CHARINDEX('_', replace(manufactureSiteInfo, 'PIN_','') + ' ') > CHARINDEX('<!>', replace(manufactureSiteInfo, 'PIN_',''), CHARINDEX('<!>', replace(manufactureSiteInfo, 'PIN_','')) + 1) 
        THEN 
            SUBSTRING(
                replace(manufactureSiteInfo, 'PIN_',''), 
                CHARINDEX('<!>', replace(manufactureSiteInfo, 'PIN_',''), CHARINDEX('<!>', replace(manufactureSiteInfo, 'PIN_','')) + 1) + 3, 
                CHARINDEX('_', replace(manufactureSiteInfo, 'PIN_','') + ' ') - CHARINDEX('<!>', replace(manufactureSiteInfo, 'PIN_',''), CHARINDEX('<!>', replace(manufactureSiteInfo, 'PIN_','')) + 1) - 3
            )
        ELSE 
            NULL
    END AS ExtractedValue
FROM 
    [192.168.8.241,15050].Zopone.dbo.POList
	