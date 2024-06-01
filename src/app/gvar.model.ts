export interface DicOfDic {
  [key: string]: { [key: string]: string };
}

export interface DicOfDT<T = any> {
  [key: string]: T[];
}

export interface GVARRequest {
  DicOfDic: DicOfDic;
}

export interface GVARResponse<T = any> {
  DicOfDic: DicOfDic;
  DicOfDT: DicOfDT<T>;
}
