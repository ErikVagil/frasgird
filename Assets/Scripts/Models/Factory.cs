using System.Collections.Generic;
public class Factory : Building {
  private List<(Good good, int amount)> inputs;
  private List<(Good good, int amount)> outputs;
  public Factory( string name,
                  int powerConsumption,
                  BuildingRequirement coreRequirement,
                  BuildingRequirement additionalRequirement,
                  (Good, int)[] inputs,
                  (Good, int)[] outputs) 
  : base(name, powerConsumption, coreRequirement, additionalRequirement) {
    this.inputs = new (inputs ?? new (Good, int)[] {});
    this.outputs = new (outputs ?? new (Good, int)[] {});
  }

  public override void OnTick() {
    foreach (var input in inputs) {
      if (Colony.Instance.GetStockpile(input.good) < input.amount) {
        return;
      }
    }
    inputs.ForEach(i => Colony.Instance.ModifyStockpile(i.good, -i.amount));
    outputs.ForEach(o => Colony.Instance.ModifyStockpile(o.good, o.amount));
  }
}