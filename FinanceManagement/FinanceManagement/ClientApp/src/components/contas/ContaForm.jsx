import './Contas.css'

import React, { useState, useEffect } from 'react';
import authService from '../api-authorization/AuthorizeService';
import { Modal, Form, ModalHeader, ModalBody, FormGroup, FormFeedback, Label, Input, Button } from 'reactstrap';
import { Icon } from 'semantic-ui-react'
import { moneyInputFormat, moneyInputFormatToFloat } from '../../helpers/FnUtils'

export default function ContaForm(props) {
    const [type, setType] = useState('tipoConta');

    const [tiposContas, setTiposContas] = useState([]);
    const [bancos, setBancos] = useState([]);

    const [tipoConta, setTipoConta] = useState(null);
    const [banco, setBanco] = useState(null);
    const [formData, setFormData] = useState({ descricao: '', saldo: 'R$0,00' });
    const [errorsDataForm, setErrorsDataForm] = useState({ descricao: false, saldo: false });

    useEffect(() => {
        getTiposContas()
        getBancos()
    }, [])

    useEffect(() => {
        if (props.contaEdit) {
            getContaEdit()
        }
    }, [props.contaEdit])

    function getContaEdit() {
        const conta = props.contaEdit
        if (conta) {
            setTipoConta(conta.tipoContaId)
            setBanco(conta.bancoId)
            setFormData({ descricao: conta.descConta, saldo: `${conta.saldo >= 0 ? `R$${moneyInputFormat(conta.saldo.toFixed(2))}` : `-R$${moneyInputFormat(conta.saldo.toFixed(2))}`}` })
        }
    }

    async function getTiposContas() {
        const token = await authService.getAccessToken();
        const user = await authService.getUser()
        const response = await fetch('api/TipoContas', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();

        setTiposContas(data);
    }

    async function getBancos() {
        const token = await authService.getAccessToken();
        const user = await authService.getUser()
        const response = await fetch('api/bancos', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();

        setBancos(data);
    }

    function renderFormTipoConta() {
        return (
            <div className='container-tipos-contas'>
                {tiposContas && tiposContas.length > 0 && tiposContas.map(tipo => (
                    <div className={`tipo-conta ${tipoConta == tipo.id ? 'tipo-active' : ''}`} key={tipo.id} onClick={() => {
                        if (tipo.tipo === 'Outros') {
                            setTipoConta(tipo.id)
                            setType('form')
                        } else {
                            setTipoConta(tipo.id)
                            setType('banco')
                        }
                    }}>
                        <span>{tipo.tipo}</span>
                    </div>
                ))}
            </div>
        )
    }

    function renderFormBanco() {
        return (
            <div>
                <span style={{ marginLeft: 10 }}>Selecione o banco</span>
                <div className='container-tipos-contas'>
                    {bancos && bancos.length > 0 && bancos.map(bank => (
                        <div className={`tipo-conta ${banco && banco == bank.id ? 'tipo-active' : ''}`} key={bank.id} onClick={() => {
                            setBanco(bank.id)
                            setType('form')
                        }}>
                            <span>{bank.nome}</span>
                        </div>
                    ))}
                </div>
            </div>
        )
    }

    const submitForm = async (e, parceladoId) => {
        e.preventDefault()

        let errors = {};

        if (!formData.descricao.length > 0) {
            errors.descricao = true
        }

        if (!formData.saldo.length > 0) {
            errors.saldo = true
        }

        if (Object.values(errors).some(err => err)) {
            setErrorsDataForm(errors)
            return null
        }

        const payload = {
            descConta: formData.descricao,
            saldo: moneyInputFormatToFloat(formData.saldo),
            data: formData.data,
            bancoId: banco,
            tipoContaId: tipoConta,
            usuarioId: props.user.sub
        }


        if (props.contaEdit) {
            const token = await authService.getAccessToken();
            const response = await fetch(`api/contas/${props.contaEdit.id}`, {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
                method: "PUT",
                body: JSON.stringify(payload)
            });

            if (response.status === 204) {
                props.getContas()
                resetForm()
                props.setShowFormConta(false)
            }
        } else {
            const token = await authService.getAccessToken();
            const response = await fetch('api/contas', {
                headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
                method: "POST",
                body: JSON.stringify(payload)
            });

            if (response.status === 201) {
                props.getContas()
                resetForm()
                props.setShowFormConta(false)
            }
        }
    }

    function renderForm() {
        return (
            <div>
                <Form onSubmit={submitForm}>
                    <FormGroup>
                        <Label for="form-description">Descrição</Label>
                        <Input
                            invalid={errorsDataForm.descricao}
                            name="description"
                            id="form-description"
                            value={formData.descricao}
                            placeholder="Insira o nome"
                            onChange={(event) => {
                                setFormData({ ...formData, descricao: event.target.value })
                                setErrorsDataForm({ ...errorsDataForm, descricao: false })
                            }}
                            onBlur={() => formData.descricao.trim().length < 1 ? setErrorsDataForm({ ...errorsDataForm, descricao: true }) : null} />
                        {errorsDataForm.descricao && <FormFeedback>Informe a descrição</FormFeedback>}
                    </FormGroup>
                    <FormGroup>
                        <Label for="form-price">Valor</Label>
                        <Input
                            name="price"
                            invalid={errorsDataForm.valor}
                            id="form-price"
                            value={formData.saldo}
                            placeholder="R$ 0,00"
                            onChange={(event) => {
                                setErrorsDataForm({ ...errorsDataForm, saldo: false })
                                setFormData({
                                    ...formData, saldo: event.target.value
                                        ? `R$${moneyInputFormat(event.target.value)}`
                                        : ''
                                })
                            }}
                            onBlur={() => formData.saldo.trim().length < 1 ? setErrorsDataForm({ ...errorsDataForm, saldo: true }) : null} />
                        {errorsDataForm.saldo && <FormFeedback>Informe o valor</FormFeedback>}
                    </FormGroup>
                    <FormGroup>
                        <Button
                            type='submit'
                            color='success'
                            style={{ width: '100%' }}
                        >
                            Salvar
                        </Button>
                    </FormGroup>
                </Form>
            </div>
        )
    }

    function resetForm() {
        setType('tipoConta')
        setTipoConta(null)
        setBanco(null)
        props.setContaEdit(null)
        setFormData({ descricao: '', saldo: 'R$0,00'})
    }

    return (
        <div>
            <Modal isOpen={props.showFormConta} toggle={() => {
                props.setShowFormConta(false)
                resetForm()
            }}>
                <ModalHeader toggle={() => {
                    props.setShowFormConta(false)
                    resetForm()
                }}>{`${props.contaEdit ? 'Editar conta' : 'Nova conta'}`}</ModalHeader>
                <ModalBody>
                    {type === 'tipoConta' && renderFormTipoConta()}
                    {type === 'banco' && renderFormBanco()}
                    {type === 'form' && renderForm()}
                </ModalBody>
            </Modal>
        </div >
    )
}