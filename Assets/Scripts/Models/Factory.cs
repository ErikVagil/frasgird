using System.Collections.Generic;
public class Factory : Building {
  private List<(Resource resource, int amount)> inputs;
  private List<(Resource resource, int amount)> outputs;
  public Factory( string name,
                  int powerConsumption,
                  (Resource, int)[] inputs,
                  (Resource, int)[] outputs) 
  : base(name, powerConsumption) {
    this.inputs = new (inputs ?? new (Resource, int)[] {});
    this.outputs = new (outputs ?? new (Resource, int)[] {});
  }

  public override void OnTick() {
    foreach (var input in inputs) {
      if (Colony.Instance.GetStockpile(input.resource) < input.amount) {
        return;
      }
    }
    inputs.ForEach(i => Colony.Instance.ModifyStockpile(i.resource, -i.amount));
    outputs.ForEach(o => Colony.Instance.ModifyStockpile(o.resource, o.amount));
  }
}