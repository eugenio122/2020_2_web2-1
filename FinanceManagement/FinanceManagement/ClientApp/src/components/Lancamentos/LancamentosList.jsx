import './Lancamentos.css';

import React, { useState, useEffect } from 'react';
import authService from '../api-authorization/AuthorizeService';
import { Button, Table, Card, Tooltip, Modal, ModalHeader, ModalBody } from 'reactstrap';
import { Segment, Icon } from 'semantic-ui-react';
import LancamentoForm from './LancamentoForm';
import { format, getMonth, addMonths } from 'date-fns';
import { moneyLabel } from '../../helpers/FnUtils';
import br from 'date-fns/locale/pt-BR';

export default function LancamentosList() {
    const [lancamentos, setLancamentos] = useState([]);
    const [loading, setLoading] = useState(false);
    const [showFormLancamento, setShowFormLancamento] = useState(false);

    const [user, setUser] = useState();

    const [lancamentoEdit, setLancamentoEdit] = useState(null);

    const [detalheLancamento, setDetalheLancamento] = useState(null);
    const [valorTotal, setValorTotal] = useState(0)

    useEffect(() => {
        getLancamentos();
        getContas();
    }, [])

    useEffect(() => {
        if (lancamentoEdit) {
            setShowFormLancamento(true)
        } else {
            setShowFormLancamento(false)
        }
    }, [lancamentoEdit])

    async function getLancamentos() {
        const token = await authService.getAccessToken();
        const user = await authService.getUser()
        const response = await fetch('api/lancamentos', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();

        if (response.status === 200) {
            setLancamentos(data);
        }

        setUser(user)
    }

    async function getContas() {
        const token = await authService.getAccessToken();
        const user = await authService.getUser()
        const response = await fetch('api/contas', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();

        if (response.status === 200) {
            const totalValue = data.reduce((sumTotal, conta) => {
                return sumTotal + conta.saldo
            }, 0)

            setValorTotal(totalValue);
        }
    }

    async function getLancamentoDetalhe(lancamento) {
        const newLancamento = { ...lancamento }

        if (lancamento.tipoLancamento === 'fixo') {
            const token = await authService.getAccessToken();
            const response = await fetch(`api/lancamentoFixoViewModel/${newLancamento.id}`, {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });

            const data = await response.json();
            setDetalheLancamento(data);
        } else if (lancamento.tipoLancamento === 'parcelado') {
            const token = await authService.getAccessToken();
            const response = await fetch(`api/lancamentoParceladoViewModel/${newLancamento.id}`, {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
            });

            const data = await response.json();
            setDetalheLancamento(data);
        } else {
            setDetalheLancamento(newLancamento)
        }
    }

    async function deleteLancamento(lancamentoId) {
        if (!window.confirm('Tem certeza que deseja excluir esse lançamento?')) return null

        const token = await authService.getAccessToken();
        const response = await fetch(`api/lancamentos/${lancamentoId}`, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` },
            method: 'DELETE'
        });

        getLancamentos()
        getContas()
        setDetalheLancamento(null)
    }

    function renderParcelas(lancamento) {
        const mesAtual = getMonth(new Date()) + 1;

        const meses = []

        for (let index = 1; index <= detalheLancamento.quantidade; index++) {
            const mes = addMonths(new Date(lancamento.data), index) + 1
            meses.push(getMonth(new Date(mes)))
        }

        console.log(meses)
        console.log(mesAtual)

        if (meses.findIndex(index => index === mesAtual) === -1) {
            return <span>Finalizado <Icon name='check' /></span>
        } else {
            return <span>Parcela {meses.findIndex(index => index === mesAtual) + 1} / {detalheLancamento.quantidade}</span>
        }
    }

    return (
        <div>
            <div className='container-header-lancamento'>
                <Segment style={{ padding: 10 }}>
                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', }}>
                        <div style={{ display: 'flex', fontSize: '18px' }}>
                            <strong style={{ marginRight: 10 }}>
                                Saldo total:
                            </strong>
                            <span className={`${valorTotal < 0 ? 'debit' : valorTotal > 0 ? 'credit' : ''}`}>
                                {`${moneyLabel(valorTotal)}`}
                            </span>
                        </div>
                        <Button color="success" onClick={() => setShowFormLancamento(true)}> Novo lançamento</Button>
                    </div>

                </Segment>
            </div>

            <div className='container-lancamentos'>
                <Segment color="green">
                    <Table responsive borderless hover>
                        <thead>
                            <tr>
                                <th>Descrição</th>
                                <th>Valor</th>
                                <th>Data</th>
                                <th>Conta</th>
                            </tr>
                        </thead>
                        <tbody>
                            {lancamentos && lancamentos.length > 0 && lancamentos.filter(lanc => lanc.status !== 'finalizado').map(lancamento => (
                                <tr key={lancamento.id} onClick={() => getLancamentoDetalhe(lancamento)}>
                                    <td>{lancamento.descricao}</td>
                                    <td className={`${lancamento.despesaReceita ? 'debit' : 'credit'}`}>
                                        {`${moneyLabel(lancamento.valor)}`} <Icon name={lancamento.despesaReceita ? 'arrow down' : 'arrow up'} />
                                    </td>
                                    <td>{format(new Date(lancamento.data), 'dd/MM/yyyy', { locale: br })}</td>
                                    <td>{lancamento.conta}</td>
                                </tr>
                            ))}
                        </tbody>
                    </Table>
                </Segment>
            </div>


            {detalheLancamento && <Modal isOpen={true} toggle={() => setDetalheLancamento(null)}>
                <ModalHeader>
                    <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                        <strong>
                            {detalheLancamento.descricao}
                        </strong>
                        <span className={`${detalheLancamento.despesaReceita ? 'debit' : 'credit'}`}>
                            {`${moneyLabel(detalheLancamento.valor)}`} <Icon name={detalheLancamento.despesaReceita ? 'arrow down' : 'arrow up'} />
                        </span>
                    </div>
                </ModalHeader>

                <ModalBody>
                    <div className='container-detalhe-lancamento'>
                        <div className='info-lancamento'>
                            <strong>Data:</strong>
                            <span>{format(new Date(detalheLancamento.data), 'dd/MM/yyyy', { locale: br })}</span>
                        </div>
                        <div className='info-lancamento'>
                            <strong>Conta:</strong>
                            <span>{detalheLancamento.conta}</span>
                        </div>
                        <div className='info-lancamento'>
                            <strong>Categoria:</strong>
                            <span>{detalheLancamento.categoria}</span>
                        </div>

                        {detalheLancamento.tipoLancamento === 'fixo' &&
                            <>
                                <hr />
                                <div className='info-fixo'>
                                    <span>Valor</span>
                                    <span>{detalheLancamento.fixo}</span>
                                    <span><Icon name='repeat' /></span>
                                </div>
                            </>
                        }
                        {detalheLancamento.tipoLancamento === 'parcelado' &&
                            <>
                                <hr />
                                <div className='info-parcelado'>
                                    <div className='desc-parcelado'>
                                        <div>
                                            <span>Repetir</span>
                                            <span>{detalheLancamento.quantidade}</span>
                                            <span>{detalheLancamento.parcelado}</span>
                                            <span><Icon name='repeat' /></span>
                                        </div>
                                        <div>
                                            {renderParcelas(detalheLancamento)}
                                        </div>
                                    </div>
                                    <div className='desc-total'>
                                        <strong>Total</strong>
                                        <span className={`${detalheLancamento.despesaReceita ? 'debit' : 'credit'}`}>
                                            {`${moneyLabel(detalheLancamento.valor * detalheLancamento.quantidade)}`} <Icon name={detalheLancamento.despesaReceita ? 'arrow down' : 'arrow up'} />
                                        </span>
                                    </div>
                                </div>
                            </>
                        }
                        <hr />
                        <div style={{ display: 'flex', justifyContent: 'space-evenly' }}>
                            {/*<Button outline color='info' onClick={() => setLancamentoEdit(detalheLancamento)}><Icon name='edit' /> Editar</Button> {" "}*/}
                            <Button outline color='danger' onClick={() => deleteLancamento(detalheLancamento.id)}><Icon name='trash' /> Excluir</Button>
                        </div>
                    </div>
                </ModalBody>
            </Modal>}


            <LancamentoForm
                showFormLancamento={showFormLancamento}
                setShowFormLancamento={setShowFormLancamento}
                getLancamentos={getLancamentos}
                getContas={getContas}
                user={user}
                lancamentoEdit={lancamentoEdit}
                setLancamentoEdit={setLancamentoEdit}
            />
        </div >
    )
}