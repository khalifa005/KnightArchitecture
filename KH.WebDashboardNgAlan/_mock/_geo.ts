import { MockRequest } from '@delon/mock';

const DATA = [
  {
    name: 'Shanghai',
    id: '310000'
  },
  {
    name: 'Municipal District',
    id: '310100'
  },
  {
    name: 'Beijing',
    id: '110000'
  },
  {
    name: 'Municipal District',
    id: '110100'
  },
  {
    name: 'Zhejiang Province',
    id: '330000'
  },
  {
    name: 'Hangzhou City',
    id: '330100'
  },
  {
    name: 'Ningbo City',
    id: '330200'
  },
  {
    name: 'Wenzhou City',
    id: '330300'
  },
  {
    name: 'Jiaxing City',
    id: '330400'
  },
  {
    name: 'Huzhou City',
    id: '330500'
  },
  {
    name: 'Shaoxing City',
    id: '330600'
  },
  {
    name: 'Jinhua City',
    id: '330700'
  },
  {
    name: 'Quzhou City',
    id: '330800'
  },
  {
    name: 'Zhoushan City',
    id: '330900'
  },
  {
    name: 'Taizhou City',
    id: '331000'
  },
  {
    name: 'Lishui City',
    id: '331100'
  }
];

export const GEOS = {
  '/geo/province': () => DATA.filter(w => w.id.endsWith('0000')),
  '/geo/:id': (req: MockRequest) => {
    const pid = (req.params.id || '310000').slice(0, 2);
    return DATA.filter(w => w.id.slice(0, 2) === pid && !w.id.endsWith('0000'));
  }
};
