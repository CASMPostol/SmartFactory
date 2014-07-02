USE IPRDEV;
GO
SELECT ipr.Title, ipr.CustomsDebtDate, ipr.DocumentNo, ipr.Grade, ipr.SKU, ipr.Batch, ipr.NetMass, ipr.AccountBalance, ipr.Cartons, ipr.OGLValidTo,
ipr.TobaccoName, ipr.InvoiceNo, ipr.DutyName, ipr.IPRDutyPerUnit, ipr.VATName, ipr.IPRVATPerUnit, ipr.GrossMass, ipr.TobaccoNotAllocated, con.Title, 
ipr.ProductivityRateMin, ipr.ProductivityRateMax,
dsp.SPNo, dsp.SADDocumentNo, dsp.SADDate, dsp.InvoiceNo, dsp.SettledQuantity, dsp.DisposalStatus, dsp.DutyPerSettledAmount, dsp.VATPerSettledAmount, dsp.DutyAndVAT,
dsp.RemainingQuantity, dsp.CustomsProcedure,
bat.Batch,
pcn.CompensationGood, pcn.ProductCodeNumber
FROM IPR AS ipr
INNER JOIN Disposal AS dsp ON ipr.ID = dsp.Disposal2IPRIndex
INNER JOIN Consent AS con ON ipr.IPR2ConsentTitle = con.Title
INNER JOIN Batch AS bat ON dsp.Disposal2BatchIndex = bat.ID
INNER JOIN PCNCode AS pcn ON dsp.Disposal2PCNID = pcn.ID
WHERE ipr.DocumentNo = '1'
ORDER BY dsp.SPNo;