namespace Archient.DesignPatterns.Gumball

module Messages =
        
    open System.Resources

    type private EmbeddedResourceAccessor() = class end
    
    let private resources = 
        // get this assembly
        let assembly = typeof<EmbeddedResourceAccessor>.Assembly

        new ResourceManager("Resources", assembly)

    module SoldOut =
        let Start = resources.GetString("SoldOut_Start")
        let Quarter = resources.GetString("SoldOut_Quarter")
        let Eject = resources.GetString("SoldOut_Eject")
        let Crank = resources.GetString("SoldOut_Crank")
        let Take = resources.GetString("SoldOut_Take")

    module Ready =
        let Start = resources.GetString("Ready_Start")
        let Quarter = resources.GetString("Ready_Quarter")
        let Eject = resources.GetString("Ready_Eject")
        let Crank = resources.GetString("Ready_Crank")
        let Take = resources.GetString("Ready_Take")

    module Quarter =
        let Quarter = resources.GetString("Quarter_Quarter")
        let Eject = resources.GetString("Quarter_Eject")
        let Crank = resources.GetString("Quarter_Crank")
        let Take = resources.GetString("Quarter_Take")

    module Crank =
        let Quarter = resources.GetString("Crank_Quarter")
        let Eject = resources.GetString("Crank_Eject")
        let Crank = resources.GetString("Crank_Crank")