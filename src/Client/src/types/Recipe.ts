import { Ingredient } from './Ingedient';
import { Step } from './Step';

export interface Recipe{
  id?: string;
  name: string;
  ingredients: Ingredient[];
  steps: Step[];
}